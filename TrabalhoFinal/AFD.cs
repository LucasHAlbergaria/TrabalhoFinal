using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoFinal
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
            estados = new HashSet<string>{"q0", "q1", "q2",};
            entrada = new HashSet<char> {'a', 'b'};
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
            Final = new HashSet<string> {"q2"};
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

    }
}
