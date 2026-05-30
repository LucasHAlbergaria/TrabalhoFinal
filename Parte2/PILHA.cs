using System;
using System.Collections.Generic;
using System.Linq;

namespace PARTE2
{
    internal class PILHA
    {
        // 7-tupla formal
        public HashSet<string> Q { get; set; } // Estados
        public HashSet<char> Sigma { get; set; } // Alfabeto de entrada
        public HashSet<char> Gamma { get; set; } // Alfabeto da pilha
        public Dictionary<(string estado, char entrada, char topoPilha), List<(string proxEstado, string addPilha)>> Delta { get; set; } // Função de transição
        public string q0 { get; set; } // Estado inicial
        public char Z0 { get; set; } // Símbolo inicial da pilha
        public HashSet<string> F { get; set; } // Estados finais (vazio, pois aceitaremos por pilha vazia)

        public PILHA()
        {
            Q = new HashSet<string>();
            Sigma = new HashSet<char>();
            Gamma = new HashSet<char>();
            Delta = new Dictionary<(string, char, char), List<(string, string)>>();
            F = new HashSet<string>(); 
        }

        public void AdicionarTransicao(string estado, char entrada, char topoPilha, string proxEstado, string addPilha)
        {
            var chave = (estado, entrada, topoPilha);
            if (!Delta.ContainsKey(chave))
                Delta[chave] = new List<(string, string)>();
            
            Delta[chave].Add((proxEstado, addPilha));
        }

        // Configuração para L2 = { a^n b^n | n >= 1 }
        public static PILHA ConstruirL2()
        {
            var PILHA = new PILHA
            {
                Q = new HashSet<string> { "q0", "q1", "q2" },
                Sigma = new HashSet<char> { 'a', 'b' },
                Gamma = new HashSet<char> { 'A', 'Z' },
                q0 = "q0",
                Z0 = 'Z'
            };

            // Empilhando 'a's
            PILHA.AdicionarTransicao("q0", 'a', 'Z', "q0", "AZ");
            PILHA.AdicionarTransicao("q0", 'a', 'A', "q0", "AA");

            // Lendo 'b's e desempilhando
            PILHA.AdicionarTransicao("q0", 'b', 'A', "q1", "\0"); // \0 representa lambda (não empilha nada, apenas desempilha)
            PILHA.AdicionarTransicao("q1", 'b', 'A', "q1", "\0");

            // Aceitação por pilha vazia no final da palavra
            PILHA.AdicionarTransicao("q1", '\0', 'Z', "q2", "\0"); // Limpa o marcador inicial da pilha

            return PILHA;
        }

        // Desafio: Configuração para L3 = Palindromos (w = w^R, |w| >= 1)
        public static PILHA ConstruirL3()
        {
            var PILHA = new PILHA
            {
                Q = new HashSet<string> { "q0", "q1", "q2" },
                Sigma = new HashSet<char> { 'a', 'b' },
                Gamma = new HashSet<char> { 'A', 'B', 'Z' },
                q0 = "q0",
                Z0 = 'Z'
            };

            char[] inputs = { 'a', 'b' };
            char[] stacks = { 'A', 'B', 'Z' };

            // q0: Fase de empilhar a primeira metade
            foreach (var s in stacks)
            {
                PILHA.AdicionarTransicao("q0", 'a', s, "q0", "A" + s);
                PILHA.AdicionarTransicao("q0", 'b', s, "q0", "B" + s);

                // Palindromos PARES (Transição Lambda)
                PILHA.AdicionarTransicao("q0", '\0', s, "q1", s.ToString());

                // Palindromos IMPARES (Lê e ignora o símbolo do meio)
                PILHA.AdicionarTransicao("q0", 'a', s, "q1", s.ToString());
                PILHA.AdicionarTransicao("q0", 'b', s, "q1", s.ToString());
            }

            // q1: Fase de desempilhar comparando a segunda metade
            PILHA.AdicionarTransicao("q1", 'a', 'A', "q1", "\0");
            PILHA.AdicionarTransicao("q1", 'b', 'B', "q1", "\0");

            // Aceitação por pilha vazia
            PILHA.AdicionarTransicao("q1", '\0', 'Z', "q2", "\0");

            return PILHA;
        }

        public bool Reconhecer(string palavra)
        {
            if (palavra.Equals("lambda", StringComparison.OrdinalIgnoreCase))
                palavra = "";

            var pilhaInicial = new Stack<char>();
            pilhaInicial.Push(Z0);

            Console.WriteLine($"\nProcessando palavra: '{(string.IsNullOrEmpty(palavra) ? "λ" : palavra)}'");
            bool aceito = Simular(palavra, 0, q0, pilhaInicial);
            
            if (aceito) Console.WriteLine(">>> RESULTADO: ACEITA\n");
            else Console.WriteLine(">>> RESULTADO: REJEITA\n");

            return aceito;
        }

        private bool Simular(string palavra, int indice, string estadoAtual, Stack<char> pilhaAtual)
        {
            // d) Exibindo Configuração Instantânea
            string stackStr = pilhaAtual.Count > 0 ? new string(pilhaAtual.ToArray()) : "[Vazia]";
            string restInput = indice < palavra.Length ? palavra.Substring(indice) : "λ";
            Console.WriteLine($"  ├─ [Estado: {estadoAtual} | Pilha: {stackStr} | Restante: {restInput}]");

            // b) Aceitação EXCLUSIVA por pilha vazia ao final da palavra
            if (pilhaAtual.Count == 0)
                return indice == palavra.Length;

            if (indice > palavra.Length) return false;

            char topo = pilhaAtual.Peek();
            char entradaAtual = indice < palavra.Length ? palavra[indice] : '\0';

            // 1. Tentar consumir símbolo da entrada
            if (entradaAtual != '\0')
            {
                var chave = (estadoAtual, entradaAtual, topo);
                if (Delta.ContainsKey(chave))
                {
                    foreach (var dest in Delta[chave])
                    {
                        var novPILHAilha = CopiarPilha(pilhaAtual);
                        novPILHAilha.Pop(); // Remove o topo consumido

                        // c) Lógica para adicionar a string na pilha (do último caractere para o primeiro)
                        for (int i = dest.addPilha.Length - 1; i >= 0; i--)
                        {
                            if (dest.addPilha[i] != '\0') // ' \0 ' representa lambda, então não empilha
                                novPILHAilha.Push(dest.addPilha[i]);
                        }

                        if (Simular(palavra, indice + 1, dest.proxEstado, novPILHAilha))
                            return true;
                    }
                }
            }

            // 2. Tentar movimentos Lambda (\0) independente da entrada atual
            var chaveLambda = (estadoAtual, '\0', topo);
            if (Delta.ContainsKey(chaveLambda))
            {
                foreach (var dest in Delta[chaveLambda])
                {
                    var novPILHAilha = CopiarPilha(pilhaAtual);
                    novPILHAilha.Pop();

                    for (int i = dest.addPilha.Length - 1; i >= 0; i--)
                    {
                        if (dest.addPilha[i] != '\0')
                            novPILHAilha.Push(dest.addPilha[i]);
                    }

                    // A transição lambda não consome a string (índice não avança)
                    if (Simular(palavra, indice, dest.proxEstado, novPILHAilha))
                        return true;
                }
            }

            return false;
        }

        private Stack<char> CopiarPilha(Stack<char> original)
        {
            var array = original.ToArray();
            Array.Reverse(array); // Reverter para manter a ordem ao repreencher
            return new Stack<char>(array);
        }
    }
}