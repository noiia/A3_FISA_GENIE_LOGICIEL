using System;
using System.IO;
using CLI.i18n;
using Config;
using Services;
using Logger;

namespace CLI
{
    public class CommandeSetLogPath : Commande
    {
        public CommandeSetLogPath() : base("Set-LogPath", new string[] { "slp", "s-lp" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[26]} {string.Join(" ", args)}");
            int r = ServiceAddSaveJob.Run(args, configuration);
            switch (r)
            {
                case ServiceSetLogPath.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {language[27]} {ConsoleColors.Reset}");
                    return;
                case ServiceSetLogPath.NOT_A_DIR:
                    Console.WriteLine($"{ConsoleColors.Red} \t{args[0]} {language[28]} {ConsoleColors.Reset}");
                    return;
                case ServiceSetLogPath.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {language[29]} {ConsoleColors.Reset}");
                    return;
            }
        }

    }
}