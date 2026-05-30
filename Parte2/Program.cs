using System;
using System.IO;

namespace PARTE2
{
    class Program
    {
        static void Main(string[] args)
        {
            string caminhoArquivo = "entradas_ap.txt";

            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"Erro: Arquivo '{caminhoArquivo}' não encontrado!");
                return;
            }

            string[] linhas = File.ReadAllLines(caminhoArquivo);

            Console.WriteLine(" TESTANDO L2: a^n b^n (Pilhas Simples) ");
            
            PILHA apL2 = PILHA.ConstruirL2();
            
            foreach (string linha in linhas)
            {
                if (!string.IsNullOrWhiteSpace(linha))
                {
                    apL2.Reconhecer(linha.Trim());
                }
            }

            Console.WriteLine(" TESTANDO L3: Palíndromos (Desafio) ");
            
            PILHA apL3 = PILHA.ConstruirL3();
            
            foreach (string linha in linhas)
            {
                if (!string.IsNullOrWhiteSpace(linha))
                {
                    string palavra = linha.Trim();
                    // O teste foca nas palavras pedidas pelo professor no PDF para L3
                    if (palavra == "a" || palavra == "aba" || palavra == "abba" || palavra == "ab" || palavra == "aab")
                    {
                        apL3.Reconhecer(palavra);
                    }
                }
            }
        }
    }
}