using System.Net.Mime;
using System.Security.Claims;

namespace EasySave
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args[0] == "--window")
            {
                //Appeller la fenetre
            }
            else
            {
                CLI.CLI cli = new CLI.CLI();
                cli.Run(args);
            }
        }
    }
}