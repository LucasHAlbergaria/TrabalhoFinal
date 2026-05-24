using TrabalhoFinal;

public class Menu
{

    public int opcao;

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
                Console.WriteLine("Você escolheu a Opção 1.");
                AFD afd = new AFD();
                Console.WriteLine("Digite uma palavra");
                string palavra = Console.ReadLine();
                afd.AceitarPalavra(palavra);
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