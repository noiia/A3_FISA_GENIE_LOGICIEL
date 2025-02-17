using System.Diagnostics;
using Config.i18n;
using Config;

using Logger;

namespace CLI
{
    public class CommandeListJobs : Commande
    {
        public CommandeListJobs() : base("list-SaveJob", new string[] { "list-job", "liste-jobs", "liste-job" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[18]}{string.Join(" ", args)}");
            
            SaveJob[] saveJobs = configuration.GetSaveJobs();
            
            
            if (saveJobs.Length == 0)
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[19]}");

            }
            foreach (var saveJob in saveJobs)
            {
                Console.WriteLine($"{ConsoleColors.Bold} {ConsoleColors.Yellow} {language[19]} {saveJob.Name} {ConsoleColors.Reset}");
                Console.WriteLine($"{ConsoleColors.Red} {language[20]} {saveJob.Id}");
                Console.WriteLine($"{ConsoleColors.Blue} {language[21]} {saveJob.LastSave}");
                Console.WriteLine($"{ConsoleColors.Cyan} {language[22]} {saveJob.Source}");
                Console.WriteLine($"{ConsoleColors.Cyan} {language[23]} {saveJob.Destination}");
                Console.WriteLine($"{ConsoleColors.Magenta} {language[24]} {saveJob.Created}");
                Console.WriteLine(ConsoleColors.Reset);
            }
        }
    }
}