using System.Globalization;
using Job.Config;
using Job.Config.i18n;
using Logger;
// using Job.Config.i18n;

namespace CLI;

public class CommandeListJobs : Commande
{
    private static Configuration _configuration;

    public CommandeListJobs() : base("list-SaveJob", new[] { "list-job", "liste-jobs", "liste-job" })
    {
    }

    public override void Action(Configuration configuration, string[] args)
    {
        var language = configuration.GetLanguage() ?? "en";
        var culture = new CultureInfo(language);
        _configuration = ConfigSingleton.Instance();
        Translation.Translator.GetString("ListSjCallWith", culture);

        LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
            $"{Translation.Translator.GetString("ListSjCallWith", culture)} {string.Join(" ", args)}");

        // Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
        // List<string> language = languageFunc();

        LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
            $"{Translation.Translator.GetString("ListSjCallWith")}{string.Join(" ", args)}");

        var saveJobs = configuration.GetSaveJobs();


        if (saveJobs.Length == 0)
            LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
                $"{Translation.Translator.GetString("NoSjToPrint")}");
        foreach (var saveJob in saveJobs)
        {
            Console.WriteLine(
                $"\t{ConsoleColors.Bold} {ConsoleColors.Yellow} {Translation.Translator.GetString("Name", culture)} {saveJob.Name} {ConsoleColors.Reset}");
            Console.WriteLine(
                $"\t\t{ConsoleColors.Red} {Translation.Translator.GetString("Id", culture)} {saveJob.Id}");
            Console.WriteLine(
                $"\t\t{ConsoleColors.Blue} {Translation.Translator.GetString("LastSave", culture)} {saveJob.LastSave}");
            Console.WriteLine(
                $"\t\t{ConsoleColors.Cyan} {Translation.Translator.GetString("Source", culture)} {saveJob.Source}");
            Console.WriteLine(
                $"\t\t{ConsoleColors.Cyan} {Translation.Translator.GetString("Destination", culture)} {saveJob.Destination}");
            Console.WriteLine(
                $"\t\t{ConsoleColors.Magenta} {Translation.Translator.GetString("Created", culture)} {saveJob.Created}");
            Console.WriteLine(ConsoleColors.Reset);
        }
    }
}