using System.Globalization;
using Job.Config;
using Job.Config.i18n;
using Logger;
// using Job.Config.i18n;

namespace CLI;

public class CommandeSetLogPath : Commande
{
    private static Configuration _configuration;

    public CommandeSetLogPath() : base("Set-LogPath", new[] { "slp", "s-lp" })
    {
    }

    public override void Action(Configuration configuration, string[] args)
    {
        // Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
        // List<string> language = languageFunc();
        var language = configuration.GetLanguage() ?? "en";
        var culture = new CultureInfo(language);

        _configuration = ConfigSingleton.Instance();

        LoggerUtility.WriteLog(_configuration.GetLogType(), LoggerUtility.Info,
            $"{Translation.Translator.GetString("SetLogPathCallWith", culture)} {string.Join(" ", args)}");

        var r = SetLogPath.Run(args, configuration);
        switch (r)
        {
            case SetLogPath.OK:
                Console.WriteLine(
                    $"{ConsoleColors.Green} {Translation.Translator.GetString("LogPathUpdatedSuccesfully", culture)} {args[0]} {ConsoleColors.Reset}");
                return;
            case SetLogPath.NOT_A_DIR:
                Console.WriteLine(
                    $"{ConsoleColors.Red} \t{args[0]} {Translation.Translator.GetString("NotValidPath", culture)} {ConsoleColors.Reset}");
                return;
            case SetLogPath.BAD_ARGS:
                Console.WriteLine(
                    $"{ConsoleColors.Red} {Translation.Translator.GetString("LogPathBadArgs", culture)} {ConsoleColors.Reset}");
                return;
        }
    }
}