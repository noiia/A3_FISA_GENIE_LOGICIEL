using System;
using System.IO;
using System.Transactions;
using CLI.i18n;
using Config;
using Services;
using Logger;

namespace CLI
{
    public class CommandeAddSaveJob : Commande
    {
        public CommandeAddSaveJob() : base("Add-SaveJob", new string[] { "asj", "a-sj" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[0]} {string.Join(" ", args)}");
            int r = ServiceAddSaveJob.Run(args, configuration);
            switch (r)
            {
                case ServiceAddSaveJob.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {language[1]} {ConsoleColors.Reset}");
                    return;
                case ServiceAddSaveJob.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {language[2]} {ConsoleColors.Reset}");
                    return;
                case ServiceAddSaveJob.NAME_ALREADY_USE:
                    Console.WriteLine($"{ConsoleColors.Red} {language[3]} ({args[0]}) {ConsoleColors.Reset}");
                    return;
                case ServiceAddSaveJob.SOURCE_DOES_NOT_EXIST:
                    Console.WriteLine($"{ConsoleColors.Red} {language[4]} ({args[1]}) {ConsoleColors.Reset}");
                    return;
                case ServiceAddSaveJob.DESTINANTION_DOES_NOT_EXIST:
                    Console.WriteLine($"{ConsoleColors.Red} {language[5]} ({args[2]}) {ConsoleColors.Reset}");
                    return;
                case ServiceAddSaveJob.TYPE_DOES_NOT_EXIST:
                    Console.WriteLine($"{ConsoleColors.Red} {language[6]} ({args[3]}) {language[7]} {ConsoleColors.Reset}");
                    return;

            }
        }

    }
}