internal class Program
{
    private static void Main(string[] args)
    {
        Menu m = new Menu();
        m.Exibir();
        m.LerOpcao();
        m.Executar();
    }
}