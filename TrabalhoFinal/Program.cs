internal class Program
{
    private static void Main(string[] args)
    {
          Menu m = new Menu();

        do{
            m.Exibir();
            m.LerOpcao();
            m.Executar();
        } while (m.opcao != 4);
    }
}