using CLI.i18n;

namespace CLI;
using System;
using System.Collections.Generic;

using Config;
using Logger;
using Services;

    public class CommandDeleteSaveJob : Commande
    {
        public CommandDeleteSaveJob() : base("delete-SaveJob", new string[] { "delete-save-job", "delete-save-jobs", "delete-jobs", "delete-savejobs", "delete-job", "delete-savejob" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, language[8]+string.Join(" ", args));

            int id = 0;
            try
            {
                id = int.Parse(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{language[9]}{args[0]})");
                return;
            }
            int r = ServiceDeleteSaveJob.Run(args, configuration);
            switch (r)
            {
                case ReturnCodes.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {language[10]} {ConsoleColors.Reset}");
                    return;
                case ReturnCodes.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {language[11]} {ConsoleColors.Reset}");
                    return;
                case ReturnCodes.JOB_DOES_NOT_EXIST:
                    Console.WriteLine($"{ConsoleColors.Red} {language[12]}{args[0]}) {ConsoleColors.Reset}");
                    return;
            }
        }
    }
