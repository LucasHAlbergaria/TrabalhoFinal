using System;
using System.IO;

namespace PARTE2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define o nome do arquivo de entradas que você enviou
            string caminhoArquivo = "entradas_ap.txt";

            // Verifica se o arquivo de texto existe na pasta do projeto
            if (!File.Exists(caminhoArquivo))
            {
                Console.WriteLine($"ERRO: O arquivo '{caminhoArquivo}' não foi encontrado!");
                Console.WriteLine("Certifique-se de que ele está na mesma pasta onde o comando 'dotnet run' está sendo executado.");
                return;
            }

            // Lê todas as linhas do arquivo de entradas
            string[] linhas = File.ReadAllLines(caminhoArquivo);

            // TESTE L2: { a^n b^n | n >= 1 }
            Console.WriteLine(" TESTE AP 1 - Linguagem L2 = { a^n b^n | n >= 1 }");
            
            // Instancia o Autômato de Pilha para a L2
            PILHA apL2 = PILHA.ConstruirL2();
            
            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue; // Ignora linhas em branco
                
                string palavra = linha.Trim();
                apL2.Reconhecer(palavra);
            }

            // TESTE L3 (DESAFIO OBRIGATÓRIO): Palíndromos
            Console.WriteLine(" TESTE AP 2 - Linguagem L3 = Palíndromos");
            
            // Instancia o Autômato de Pilha para a L3
            PILHA apL3 = PILHA.ConstruirL3();
            
            foreach (var linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha)) continue;
                
                string palavra = linha.Trim();
                
                // Filtramos as palavras para testar L3 conforme a sua especificação (opcional)
                // Caso queira testar todas as palavras para L3, basta remover esse 'if'
                if (palavra == "a" || palavra == "aba" || palavra == "abba" || palavra == "ab" || palavra == "aab")
                {
                    apL3.Reconhecer(palavra);
                }
            }

            Console.WriteLine("Fim do processamento. Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}