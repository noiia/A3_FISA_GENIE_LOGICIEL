// See https://aka.ms/new-console-template for more information

namespace EasySaveCLI
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
                CLI.CLI cli = new CLI.CLI();
                cli.Run(args);
        }
    }
}