using System;
using System.IO;
using Config.i18n;

using Config;
using Job.Config;
using Job.Config.i18n;
using Logger;

namespace CLI
{
    public class CommandeSetLanguage : Commande
    {
        public CommandeSetLanguage() : base("Set-Language", new string[] { "sl" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            int r = SetLanguage.Run(args, configuration);
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            switch (r)
            {
                case SetLanguage.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {language[29]} {args[0]} {ConsoleColors.Reset}");
                    return;
                case SetLanguage.NOT_A_LANGUAGE:
                    Console.WriteLine($"{ConsoleColors.Red} \t{args[0]} {language[30]} {ConsoleColors.Reset}");
                    return;
                case SetLanguage.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {language[31]} {string.Join(" ", args)} {ConsoleColors.Reset}");
                    return;
            }
        }

    }
}