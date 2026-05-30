using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Parte1
{
    internal class AFD
    {

        public HashSet<string> estados;

        public HashSet<char> entrada;

        public Dictionary<(string estado, char simbolo), string> transicoes;

        public string inicial;

        public HashSet<string> Final;


        public AFD()
        {
            estados = new HashSet<string> { "q0", "q1", "q2", };
            entrada = new HashSet<char> { 'a', 'b' };
            transicoes = new Dictionary<(string estado, char simbolo), string>
            {
                { ("q0", 'a'), "q1" },
                { ("q0", 'b'), "q0" },
                { ("q1", 'a'), "q1" },
                { ("q1", 'b'), "q2" },
                { ("q2", 'a'), "q1" },
                { ("q2", 'b'), "q0" },
            };
            inicial = "q0";
            Final = new HashSet<string> { "q2" };
        }


        //Desafio: receber qualquer AFD
        public AFD(HashSet<string> estados, HashSet<char> entrada, Dictionary<(string estado, char simbolo), string> transicoes, string inicial, HashSet<string> Final)
        {
            this.estados = estados;
            this.entrada = entrada;
            this.transicoes = transicoes;
            this.inicial = inicial;
            this.Final = Final;
        }

        public AFD Desafio(string caminhoJson)
        {
            string caminhoReal = caminhoJson;
            if (!File.Exists(caminhoReal))
            {
                string alternativa = Path.Combine(AppContext.BaseDirectory, caminhoJson);
                if (File.Exists(alternativa)) caminhoReal = alternativa;
            }

            if (!File.Exists(caminhoReal))
            {
                Console.WriteLine($"Arquivo '{caminhoJson}' não encontrado.");
                return this;
            }

            try
            {
                string jsonContent = File.ReadAllText(caminhoReal);
                using var doc = JsonDocument.Parse(jsonContent);
                var root = doc.RootElement;

                // estados
                var estados = new HashSet<string>();
                if (root.TryGetProperty("estados", out var pEstados) && pEstados.ValueKind == JsonValueKind.Array)
                {
                    foreach (var e in pEstados.EnumerateArray())
                        estados.Add(e.GetString() ?? string.Empty);
                }

                // alfabeto / entrada
                var entrada = new HashSet<char>();
                if (root.TryGetProperty("alfabeto", out var pAlfabeto) && pAlfabeto.ValueKind == JsonValueKind.Array)
                {
                    foreach (var a in pAlfabeto.EnumerateArray())
                    {
                        var s = a.GetString();
                        if (!string.IsNullOrEmpty(s)) entrada.Add(s[0]);
                    }
                }

                // estado inicial
                string inicialJson = inicial;
                if (root.TryGetProperty("estadoInicial", out var pInicial) && pInicial.ValueKind == JsonValueKind.String)
                    inicialJson = pInicial.GetString() ?? inicialJson;

                // estados finais (tenta várias chaves possíveis)
                var finais = new HashSet<string>();
                if (root.TryGetProperty("estadoFinal", out var pFinal) && pFinal.ValueKind == JsonValueKind.Array)
                {
                    foreach (var f in pFinal.EnumerateArray()) finais.Add(f.GetString() ?? string.Empty);
                }
                else if (root.TryGetProperty("estadosAceitacao", out var pAce) && pAce.ValueKind == JsonValueKind.Array)
                {
                    foreach (var f in pAce.EnumerateArray()) finais.Add(f.GetString() ?? string.Empty);
                }

                // transições: espera array de objetos { de, letra, para }
                var transicoes = new Dictionary<(string estado, char simbolo), string>();
                if (root.TryGetProperty("transicoes", out var pTrans) && pTrans.ValueKind == JsonValueKind.Array)
                {
                    foreach (var t in pTrans.EnumerateArray())
                    {
                        if (!t.TryGetProperty("de", out var pDe) || !t.TryGetProperty("letra", out var pLetra) || !t.TryGetProperty("para", out var pPara))
                            continue;

                        string de = pDe.GetString() ?? string.Empty;
                        string letraStr = pLetra.GetString() ?? string.Empty;
                        if (string.IsNullOrEmpty(letraStr)) continue;
                        char simbolo = letraStr[0];
                        string para = pPara.GetString() ?? string.Empty;

                        transicoes[(de, simbolo)] = para;
                    }
                }

                var afdCustom = new AFD(estados, entrada, transicoes, inicialJson, finais);
                Console.WriteLine("AFD personalizado carregado com sucesso (apenas para o desafio).\n");
                return afdCustom;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler ou processar o JSON: {ex.Message}");
                return this;
            }
        }

        public bool AceitarPalavra(string palavra)
        {
            string estadoAtual = inicial;

            foreach (char simbolo in palavra)
            {

                if (!entrada.Contains(simbolo))
                {
                    Console.WriteLine($"Símbolo '{simbolo}' não reconhecido no alfabeto. A palavra é rejeitada.");
                    return false;
                }

                var passoAtual = (estadoAtual, simbolo);

                if (transicoes.ContainsKey(passoAtual))
                {

                    estadoAtual = transicoes[passoAtual];
                }
                else
                {

                    Console.WriteLine($"Transição indefinida para o estado '{estadoAtual}' com o símbolo '{simbolo}'. Palavra rejeitada.");
                    return false;
                }
            }


            if (Final.Contains(estadoAtual))
            {
                Console.WriteLine($"A palavra '{palavra}' terminou no estado de aceitação '{estadoAtual}'. ACEITA!");
                return true;
            }
            else
            {
                Console.WriteLine($"A palavra '{palavra}' terminou no estado '{estadoAtual}', que NÃO é de aceitação. REJEITADA!");
                return false;
            }
        }


        public void CarregarPalavrasDeArquivo(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"Arquivo '{caminhoArquivo}' não encontrado.");
                return;
            }

            string[] linhas = File.ReadAllLines(caminhoArquivo);
            int numero = 0;

            foreach (var raw in linhas)
            {
                numero++;

                if (raw == null) continue;

                string trimmed = raw.Trim();


                bool isLambda = trimmed.IndexOf("lambda", StringComparison.OrdinalIgnoreCase) >= 0 || trimmed.Contains('λ');
                string palavra = isLambda || string.IsNullOrEmpty(trimmed) ? string.Empty : trimmed;

                if (string.IsNullOrEmpty(trimmed))
                {
                    Console.WriteLine($"Teste {numero}: linha vazia -> usando palavra vazia");
                }
                else if (isLambda)
                {
                    Console.WriteLine($"Teste {numero}: '{raw}' -> tratando como palavra vazia");
                }
                else
                {
                    Console.WriteLine($"Teste {numero}: '{raw}'");
                }

                AceitarPalavra(palavra);
            }
        }


        public void ExibirAFD()
        {
           
            string listaEstados = string.Join(", ", estados);
            string listaAlfabetos = string.Join(", ", entrada);

            Console.WriteLine($"Estados: [{listaEstados}]");
            Console.WriteLine($"Estados: [{listaAlfabetos}]");
            Console.WriteLine("Transições registradas (Função Delta):");

            foreach (var transicao in transicoes)
            {
               
                string estadoOrigem = transicao.Key.estado;
                char simbolo = transicao.Key.simbolo;

               
                string estadoDestino = transicao.Value;

                
                Console.WriteLine($"  δ({estadoOrigem}, '{simbolo}') = {estadoDestino}");
            }

                Console.WriteLine($"Estado Inicial: {inicial}");
                string listaFinais = string.Join(", ", Final);
                Console.WriteLine($"Estados de Aceitação: [{listaFinais}]");
        }
    }
}
