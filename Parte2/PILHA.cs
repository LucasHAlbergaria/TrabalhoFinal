using System;
using System.Collections.Generic;

namespace PARTE2
{
    public class Transicao
    {
        public string De = "";          // Estado de origem (agora inicializado com vazio)
        public char Le;                 // Letra lida da palavra 
        public char Desempilha;         // O que precisa estar no topo da pilha
        public string Para = "";        // Estado de destino (agora inicializado com vazio)
        public string Empilha = "";     // O que vai colocar na pilha (agora inicializado)
    }

    internal class PILHA
    {
        public string Inicial = "";     // Inicializado com vazio para remover o aviso
        public char FundoPilha = 'Z';
        public List<Transicao> Transicoes = new List<Transicao>();

        public PILHA()
        {
            // Construtor vazio
        }

        // Método para registrar uma regra na nossa lista de transições
        public void AdicionarTransicao(string de, char le, char desempilha, string para, string empilha)
        {
            Transicoes.Add(new Transicao { De = de, Le = le, Desempilha = desempilha, Para = para, Empilha = empilha });
        }

        // Método principal que chama a simulação
        public void Reconhecer(string palavra)
        {
            // Trata a palavra escrita "lambda" no arquivo txt como uma palavra vazia real
            if (palavra.ToLower() == "lambda") palavra = "";

            Console.WriteLine($"\nProcessando palavra: '{(palavra == "" ? "λ" : palavra)}'");

            // Prepara a pilha inicial e coloca o Z no fundo
            Stack<char> pilhaInicial = new Stack<char>();
            pilhaInicial.Push(FundoPilha);

            // Inicia a verificação a partir do índice 0 e do estado Inicial
            bool aceito = TentarCaminhos(palavra, 0, Inicial, pilhaInicial);

            if (aceito)
                Console.WriteLine(">>> RESULTADO: ACEITA\n");
            else
                Console.WriteLine(">>> RESULTADO: REJEITA\n");
        }

        // Método que tenta encontrar o caminho certo (Simulação)
        private bool TentarCaminhos(string palavra, int posicaoAtual, string estadoAtual, Stack<char> pilhaAtual)
        {
            // 1. Mostrar o estado das coisas no console (Configuração Instantânea)
            string conteudoPilha = string.Join("", pilhaAtual); // Transforma a pilha em texto
            if (conteudoPilha == "") conteudoPilha = "[Vazia]";
            
            string restantePalavra = posicaoAtual < palavra.Length ? palavra.Substring(posicaoAtual) : "λ";
            Console.WriteLine($"  ├─ Estado: {estadoAtual} | Pilha: {conteudoPilha} | Restante: {restantePalavra}");

            // 2. Condição de parada e aceitação: A pilha esvaziou!
            if (pilhaAtual.Count == 0)
            {
                // Se a pilha esvaziou e a palavra foi toda lida, então ACEITA (retorna true)
                return posicaoAtual == palavra.Length; 
            }

            // Descobre qual é a letra que estamos lendo agora ('-' se a palavra já acabou)
            char letraAtual = '-';
            if (posicaoAtual < palavra.Length)
            {
                letraAtual = palavra[posicaoAtual];
            }

            char topoPilha = pilhaAtual.Peek(); // Apenas olha o topo da pilha

            // 3. Testa todas as transições que cadastramos
            foreach (var t in Transicoes)
            {
                // Verifica se a transição serve para o estado atual e o topo da pilha atual
                if (t.De == estadoAtual && t.Desempilha == topoPilha)
                {
                    // A transição serve se ela pede a letra atual, OU se ela é uma transição lambda (vazia = '-')
                    bool consomeLetra = (t.Le == letraAtual && letraAtual != '-');
                    bool movimentoVazio = (t.Le == '-');

                    if (consomeLetra || movimentoVazio)
                    {
                        // Cria uma cópia da pilha para este caminho (jeito simples com arrays)
                        char[] arrayPilha = pilhaAtual.ToArray();
                        Array.Reverse(arrayPilha);
                        Stack<char> novaPilha = new Stack<char>(arrayPilha);

                        // Aplica a regra: desempilha o topo
                        novaPilha.Pop();

                        // Aplica a regra: empilha o que a transição mandou (se não for '-')
                        if (t.Empilha != "-")
                        {
                            // Empilha de trás pra frente (ex: "AZ", empilha Z, depois A)
                            for (int i = t.Empilha.Length - 1; i >= 0; i--)
                            {
                                novaPilha.Push(t.Empilha[i]);
                            }
                        }

                        // Se consumiu letra, avança a posição. Se foi vazio ('-'), continua na mesma posição
                        int novaPosicao = consomeLetra ? posicaoAtual + 1 : posicaoAtual;

                        // Tenta continuar por esse caminho! Se esse caminho der certo (true), finaliza com sucesso
                        if (TentarCaminhos(palavra, novaPosicao, t.Para, novaPilha))
                        {
                            return true;
                        }
                    }
                }
            }

            // Se testou todas as regras e nenhuma levou ao fim, este caminho é rejeitado
            return false;
        }

        public static PILHA ConstruirL2()
        {
            PILHA ap = new PILHA();
            ap.Inicial = "q0";

            // Lendo 'a' e empilhando 'A'
            ap.AdicionarTransicao(de: "q0", le: 'a', desempilha: 'Z', para: "q0", empilha: "AZ");
            ap.AdicionarTransicao(de: "q0", le: 'a', desempilha: 'A', para: "q0", empilha: "AA");

            // Começando a ler 'b' (não empilha nada, representado por '-')
            ap.AdicionarTransicao(de: "q0", le: 'b', desempilha: 'A', para: "q1", empilha: "-");

            // Continuar lendo 'b' e desempilhando
            ap.AdicionarTransicao(de: "q1", le: 'b', desempilha: 'A', para: "q1", empilha: "-");

            // Quando terminar, usa um movimento vazio '-' para tirar o Z do fundo e aceitar por pilha vazia
            ap.AdicionarTransicao(de: "q1", le: '-', desempilha: 'Z', para: "q2", empilha: "-");

            return ap;
        }

        public static PILHA ConstruirL3()
        {
            PILHA ap = new PILHA();
            ap.Inicial = "q0";

            char[] toposPossiveis = { 'Z', 'A', 'B' };

            foreach (char topo in toposPossiveis)
            {
                // FASE 1: Empilhar tudo o que vier
                ap.AdicionarTransicao("q0", 'a', topo, "q0", "A" + topo);
                ap.AdicionarTransicao("q0", 'b', topo, "q0", "B" + topo);

                // FASE 2: Adivinhar o meio da palavra para Palíndromos PARES (movimento vazio '-')
                ap.AdicionarTransicao("q0", '-', topo, "q1", topo.ToString());

                // FASE 2: Adivinhar o meio da palavra para Palíndromos ÍMPARES (lê a letra do meio e ignora ela)
                ap.AdicionarTransicao("q0", 'a', topo, "q1", topo.ToString());
                ap.AdicionarTransicao("q0", 'b', topo, "q1", topo.ToString());
            }

            // FASE 3: Desempilhar verificando se a segunda metade é igual
            ap.AdicionarTransicao("q1", 'a', 'A', "q1", "-");
            ap.AdicionarTransicao("q1", 'b', 'B', "q1", "-");

            // Fim: remover o fundo da pilha para aceitar
            ap.AdicionarTransicao("q1", '-', 'Z', "q2", "-");

            return ap;
        }
    }
}