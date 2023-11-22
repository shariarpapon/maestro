using Maestro;

public static class Program 
{
    public static void Main() 
    {
        RWProvider rwProvider = new RWProvider(Console.Write, Console.WriteLine, Console.ReadLine!);
        MaestroTerminal.InitiateThread("maestro testbed", rwProvider, null!);
    }
}