using System;
using System.IO;
using CLI.i18n;
using Logger;


namespace CLI
{
    public class CommandeExecSaveJob : Commande
    {
        
        //TODO : ajouté le temps du fichier delta du cp 
        public CommandeExecSaveJob() : base("Exec-SaveJob", new string[] { "execsj", "e-sj" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[13]} {string.Join(" ", args)}");
            int intType;
            int r;
            if(args[0].Contains(";"))
            {
                string[] listIds = args[0].Split(';');
                foreach (var id in listIds)
                {
                    string[] arg = [id];
                    r = ServiceExecSaveJob.Run(arg, configuration);
                    switch (r)
                    {
                        case ServiceExecSaveJob.OK:
                            Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                            return;
                        case ServiceExecSaveJob.BAD_ARGS:
                            Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                            return;
                        case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                            Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                            return;
                    }
                }
            }
            else if(args[0].Contains(","))
            {
                int min = int.Parse(args[1].Split(',')[0]);
                int max = int.Parse(args[1].Split(',')[1]);
                for (int i = min; i <= max; i++)
                {
                    string[] arg = [i.ToString()];
                    r = ServiceExecSaveJob.Run(args, configuration);
                    switch (r)
                    {
                        case ServiceExecSaveJob.OK:
                            Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                            return;
                        case ServiceExecSaveJob.BAD_ARGS:
                            Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                            return;
                        case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                            Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                            return;
                    } 
                }

            }
            else if(int.TryParse(args[0], out intType))
            {
                r = ServiceExecSaveJob.Run(args, configuration);
                switch (r)
                {
                    case ServiceExecSaveJob.OK:
                        Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                        return;
                    case ServiceExecSaveJob.BAD_ARGS:
                        Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                        return;
                    case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                        Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                        return;
                }   
            }
            else
            {
                Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
            }
        }

    }
}