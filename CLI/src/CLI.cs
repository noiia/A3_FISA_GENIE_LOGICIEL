using System;
using System.Linq;

namespace CLI
{
    public class CLI
    {
        private static readonly string[] processList = { "list-SaveJob", "add-SaveJob", "delete-SaveJob", "exec-SaveJob", "help" };
        
        private static Commande[] _commandes;
        
        public CLI()
        {
            _commandes = Array.Empty<Commande>();
            _commandes = _commandes.Append(new CommandeHelp()).ToArray();
            _commandes = _commandes.Append(new CommandeListJobs()).ToArray();
            _commandes = _commandes.Append(new CommandeAddSaveJob()).ToArray();
        }

        public void Run(string[] args)
        {
            DisplayEasySave();
            if (args.Length == 0)
            {
                foreach (Commande c in _commandes)
                {
                    if (c.IsCall("Help"))
                    {
                        c.Run(null);
                    }
                } 
            }
            else
            {
                foreach (Commande c in _commandes)
                {
                    if (c.IsCall(args[0]))
                    {
                        c.Run(args.Skip(1).ToArray());
                    }
                }   
            }
        }

        public static void DisplayEasySave()
        {
            Console.Write(ConsoleColors.Cyan);
            Console.WriteLine("===========================================================");
            Console.WriteLine("  ______                      _____                    ");
            Console.WriteLine(" |  ____|                    / ____|                   ");
            Console.WriteLine(" | |__    __ _  ___  _   _  | (___    __ _ __   __ ___ ");
            Console.WriteLine(" |  __|  / _` |/ __|| | | |  \\___ \\  / _` |\\ \\ / // _ \\");
            Console.WriteLine(" | |____| (_| |\\__ \\| |_| |  ____) || (_| | \\ V /|  __/");
            Console.WriteLine(" |______|\\__,_||___/ \\__, | |_____/  \\__,_|  \\_/  \\___|.exe");
            Console.WriteLine("                      __/ |                            ");
            Console.WriteLine("                     |___/                             ");
            Console.WriteLine("===========================================================");
            Console.Write(ConsoleColors.Reset);

        }
    }
}