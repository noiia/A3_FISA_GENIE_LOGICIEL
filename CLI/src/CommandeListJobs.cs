using System.Diagnostics;
using System.Globalization;
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
            string language = configuration.GetLanguage() ?? "en"; 
            CultureInfo culture = new CultureInfo(language);

            Translation.Translator.GetString("ListSjCallWith", culture);

            LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("ListSjCallWith", culture)} {string.Join(" ", args)}");
            
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
                Console.WriteLine($"\t{ConsoleColors.Bold} {ConsoleColors.Yellow} {Translation.Translator.GetString("Name", culture )} {saveJob.Name} {ConsoleColors.Reset}");
                Console.WriteLine($"\t\t{ConsoleColors.Red} {Translation.Translator.GetString("Id", culture)} {saveJob.Id}");
                Console.WriteLine($"\t\t{ConsoleColors.Blue} {Translation.Translator.GetString("LastSave", culture)} {saveJob.LastSave}");
                Console.WriteLine($"\t\t{ConsoleColors.Cyan} {Translation.Translator.GetString("Source", culture)} {saveJob.Source}");
                Console.WriteLine($"\t\t{ConsoleColors.Cyan} {Translation.Translator.GetString("Destination", culture)} {saveJob.Destination}");
                Console.WriteLine($"\t\t{ConsoleColors.Magenta} {Translation.Translator.GetString("Created", culture)} {saveJob.Created}");
                Console.WriteLine(ConsoleColors.Reset);
            }
        }
    }
}