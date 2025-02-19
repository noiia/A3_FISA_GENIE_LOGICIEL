using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Config.i18n;

using Config;
using Job.Config;
using Job.Config.i18n;
// using Job.Config.i18n;
using Logger;

namespace CLI
{
    public class CommandeSetLogPath : Commande
    {
        public CommandeSetLogPath() : base("Set-LogPath", new string[] { "slp", "s-lp" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            // Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            // List<string> language = languageFunc();
            string language = configuration.GetLanguage() ?? "en"; 
            CultureInfo culture = new CultureInfo(language);
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{Translation.Translator.GetString("SetLogPathCallWith", culture)} {string.Join(" ", args)}");
            
            int r = SetLogPath.Run(args, configuration);
            switch (r)
            {
                case SetLogPath.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {Translation.Translator.GetString("LogPathUpdatedSuccesfully", culture)} {args[0]} {ConsoleColors.Reset}");
                    return;
                case SetLogPath.NOT_A_DIR:
                    Console.WriteLine($"{ConsoleColors.Red} \t{args[0]} {Translation.Translator.GetString("NotValidPath", culture)} {ConsoleColors.Reset}");
                    return;
                case SetLogPath.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {Translation.Translator.GetString("LogPathBadArgs", culture)} {ConsoleColors.Reset}");
                    return;
            }
        }

    }
}