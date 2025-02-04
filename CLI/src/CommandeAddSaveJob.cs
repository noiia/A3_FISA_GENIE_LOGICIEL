using System;
using System.IO;

namespace EasySave
{
    public class CommandeAddSaveJob : Commande
    {
        public CommandeAddSaveJob() : base("Add-SaveJob", new string[] { "asj", "a-sj" }) { }

        public override void Action(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            if (args.Length == 3)
            {
                Configuration configuration = new Configuration( Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EsaySave\\" + "config.json");
                configuration.LoadConfiguration();
                SaveJob saveJob = configuration.GetSaveJob(args[0]);
                if (saveJob != null)
                {
                    Console.WriteLine(ConsoleColors.Red + "\t A save Job already exist with this name (" + args[0] + ")" + ConsoleColors.Reset);
                    return;
                }
                if (!Directory.Exists(args[1]))
                {
                    Console.WriteLine(ConsoleColors.Red + "Source directory does not exist ("  + args[1] + " )" + ConsoleColors.Reset);
                    return;
                }
                if (!Directory.Exists(args[2]))
                {
                    Console.WriteLine(ConsoleColors.Red + "Destination directory does not exist ( " + args[2] + " )" + ConsoleColors.Reset);
                    return;
                }
            }
            else
            {
                Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Add-Savejob <name> <source> <destination>" + ConsoleColors.Reset);
            }
        }

    }
}