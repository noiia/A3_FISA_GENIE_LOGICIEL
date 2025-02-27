using System.Globalization;
using Job.Config;
using Job.Config.i18n;
// using Job.Config.i18n;

namespace CLI;

public class CommandeSetLanguage : Commande
{
    public CommandeSetLanguage() : base("Set-Language", new[] { "sl" })
    {
    }

    public override void Action(Configuration configuration, string[] args)
    {
        var r = SetLanguage.Run(args, configuration);
        // Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
        // List<string> language = languageFunc();

        var language = configuration.GetLanguage() ?? "en";
        var culture = new CultureInfo(language);

        switch (r)
        {
            case SetLanguage.OK:
                Console.WriteLine(
                    $"{ConsoleColors.Green} {Translation.Translator.GetString("LanguageChanged", culture)} {args[0]} {ConsoleColors.Reset}");
                return;
            case SetLanguage.NOT_A_LANGUAGE:
                Console.WriteLine(
                    $"{ConsoleColors.Red} \t{args[0]} {Translation.Translator.GetString("LanguageNotSupported", culture)} {ConsoleColors.Reset}");
                return;
            case SetLanguage.BAD_ARGS:
                Console.WriteLine(
                    $"{ConsoleColors.Red} {Translation.Translator.GetString("LanguageBadArgs", culture)} {string.Join(" ", args)} {ConsoleColors.Reset}");
                return;
        }
    }
}