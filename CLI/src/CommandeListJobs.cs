using System;
using Services;

namespace EasySave
{
    public class CommandeListJobs : Commande
    {
        public CommandeListJobs() : base("list-SaveJob", new string[] { "list-job", "liste-jobs", "liste-job" }) { }

        public override void Action(string[] args)
        {
            SaveJob[] saveJobs = ServiceListSaveJob.Run(args);
            foreach (var saveJob in saveJobs)
            {
                Console.WriteLine(ConsoleColors.Bold + ConsoleColors.Yellow + "\tName : " + saveJob.Name + ConsoleColors.Reset);
                Console.WriteLine( ConsoleColors.Red  + "\t\tId : "+ saveJob.Id );
                Console.WriteLine( ConsoleColors.Blue  + "\t\tLast save : "+ saveJob.LastSave );
                Console.WriteLine( ConsoleColors.Cyan  + "\t\tSource : "+ saveJob.Source );
                Console.WriteLine( ConsoleColors.Cyan  + "\t\tDestination : "+ saveJob.Destination );
                Console.WriteLine( ConsoleColors.Magenta  + "\t\tCreated : "+ saveJob.Created );
                Console.WriteLine(ConsoleColors.Reset);
            }
        }

    }
}