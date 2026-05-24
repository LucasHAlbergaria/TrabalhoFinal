using TrabalhoFinal;

public class Menu
{

    public int opcao;
    AFD afd = new AFD();

    public void Exibir()
    {
        Console.WriteLine("Bem-vindo ao Menu!");
        Console.WriteLine("1. AFD");
        Console.WriteLine("2. Pilha");
        Console.WriteLine("3. Máquina de Turing");
        Console.WriteLine("4. Sair");

        
    }

    public void LerOpcao()
    {
        Console.Write("Digite a opção desejada: ");
        int.TryParse(Console.ReadLine(), out opcao);

        if (opcao < 1 || opcao > 4 || opcao == 0) 
        {
            Console.WriteLine("Opção inválida. Por favor, tente novamente.");
            LerOpcao(); // Chama o método novamente para ler uma opção válida
        }

    }

    public void Executar()
    {
        switch (opcao)
        {
            case 1:

                Console.Clear();
                Console.WriteLine("Você escolheu a Opção 1.");
                Console.WriteLine("1. Teste de palavra desejada AFD");
                Console.WriteLine("2. Testes de aceitação de palavras (slide)");
                Console.WriteLine("3. Desafio: Ler Json com AFD");
                Console.WriteLine("4. Exibir o AFD");
                Console.Write("Escolha uma opção: ");
                int.TryParse(Console.ReadLine(), out int subOpcao);

   

                if (subOpcao == 1)
                {
                    Console.WriteLine("Digite uma palavra");
                    string palavra = Console.ReadLine() ?? string.Empty;
                    afd.AceitarPalavra(palavra);
                }
                else if (subOpcao == 2)
                {
                    string caminhoArquivo = "entradasAFD.txt";
                    Console.WriteLine($"Lendo arquivo de testes: {caminhoArquivo}");
                    afd.CarregarPalavrasDeArquivo(caminhoArquivo);
                    Console.WriteLine("Testes concluídos. Aperte uma tecla para limpar o console e continuar");
                    Console.ReadKey();
                    Console.Clear();
                }
                else if (subOpcao == 3)
                {
                   
                    afd =  afd.Desafio("afd.json");
                    Console.WriteLine("Digite uma palavra");
                    string palavra = Console.ReadLine() ?? string.Empty;
                    afd.AceitarPalavra(palavra);
                    Console.WriteLine("Testes concluídos. Aperte uma tecla para limpar e continuar");
                    Console.ReadKey();
                    Console.Clear();

                }
                else if (subOpcao == 4)
                {

                    afd.ExibirAFD();
                    Console.WriteLine("AFD escrito com sucesso. Aperte uma tecla para limpar o console e continuar");
                    Console.ReadKey();
                    Console.Clear();

                }
                else
                {
                    Console.WriteLine("Opção inválida. Retornando ao menu principal.");
                }
                break;
            case 2:
                Console.WriteLine("Você escolheu a Opção 2.");
                break;
            case 3:
                Console.WriteLine("Você escolheu a Opção 3.");
                
                break;
            case 4:
                Console.WriteLine("Saindo do programa. Até logo!");
                break;
            default:
                Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                break;
        }
    }
}