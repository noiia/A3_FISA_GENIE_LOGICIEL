using System.Diagnostics;
using Config.i18n;
using Config;
using Job.Config;
using Job.Config.i18n;
// using Job.Config.i18n;
using Logger;

namespace CLI
{
    public class CommandeListJobs : Commande
    {
        public CommandeListJobs() : base("list-SaveJob", new string[] { "list-job", "liste-jobs", "liste-job" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            
            // Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            // List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("ListSjCallWith")}{string.Join(" ", args)}");
            
            SaveJob[] saveJobs = configuration.GetSaveJobs();
            
            
            if (saveJobs.Length == 0)
            {
                LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("NoSjToPrint")}");
        
            }
            foreach (var saveJob in saveJobs)
            {
                Console.WriteLine($"{ConsoleColors.Bold} {ConsoleColors.Yellow} {Translation.Translator.GetString("Name")} {saveJob.Name} {ConsoleColors.Reset}");
                // Console.WriteLine($"{ConsoleColors.Red} {language[20]} {saveJob.Id}");
            }
        }
    }
}