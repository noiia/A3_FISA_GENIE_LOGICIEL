using Config;
using Services;
using Logger;

namespace CLI
{
    public class CommandeListJobs : Commande
    {
        public CommandeListJobs() : base("list-SaveJob", new string[] { "list-job", "liste-jobs", "liste-job" }) { }

        public override void Action(string[] args)
        {
            LoggerUtility.WriteLog(LoggerUtility.Info, "List-SaveJob : is call with args : "+string.Join(" ", args));

            SaveJob[] saveJobs = ServiceListSaveJob.Run(args);
            if (saveJobs.Length == 0)
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, "There is no SaveJob to print");

            }
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