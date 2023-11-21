using Maestro;

public static class Program 
{
    public static void Main() 
    {
        Thread thread = new Thread(new ThreadStart(() => { MaestroTerminal.Initiate("maestro testbead"); }));
        thread.Start();
        while (!MaestroTerminal.IsReady)
            continue;
    }
}