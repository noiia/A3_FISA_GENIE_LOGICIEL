using System;
using System.IO;
using CLI.i18n;

using Config;
using Logger;

namespace CLI
{
    public class CommandeSetLanguage : Commande
    {
        public CommandeSetLanguage() : base("Set-Language", new string[] { "sl" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            //TODO: Add a log message LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[26]} {string.Join(" ", args)}");
            int r = ServiceSetLanguage.Run(args, configuration);
            switch (r)
            {
                case ServiceSetLanguage.OK:
                    // TODO: Set message Console.WriteLine($"{ConsoleColors.Green} {language[27]} {ConsoleColors.Reset}");
                    return;
                case ServiceSetLanguage.NOT_A_LANGUAGE:
                    //TODO: Set message Console.WriteLine($"{ConsoleColors.Red} \t{args[0]} {language[28]} {ConsoleColors.Reset}");
                    return;
                case ServiceSetLanguage.BAD_ARGS:
                    //TODO: Set message SerConsole.WriteLine($"{ConsoleColors.Red} {language[29]} {ConsoleColors.Reset}");
                    return;
            }
        }

    }
}