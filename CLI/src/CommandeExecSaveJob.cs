using System;
using System.IO;
using Services;
using Logger;


namespace CLI
{
    public class CommandeExecSaveJob : Commande
    {
        public CommandeExecSaveJob() : base("Exec-SaveJob", new string[] { "execsj", "e-sj" }) { }

        public override void Action(string[] args)
        {
            LoggerUtility.WriteLog(LoggerUtility.Info, "Exec-SaveJob : is call with args : "+string.Join(" ", args));
            int intType;
            int r;
            if(args[0].Contains(";"))
            {
                string[] listIds = args[0].Split(';');
                foreach (var id in listIds)
                {
                    string[] arg = [id];
                    r = ServiceExecSaveJob.Run(arg);
                    switch (r)
                    {
                        case ServiceExecSaveJob.OK:
                            Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn created" + ConsoleColors.Reset);
                            return;
                        case ServiceExecSaveJob.BAD_ARGS:
                            Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Exec-Savejob <id> | <id>;<id>;<id> | <id>,<id>" + ConsoleColors.Reset);
                            return;
                        case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                            Console.WriteLine(ConsoleColors.Red + "\t This job does not exist" + ConsoleColors.Reset);
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
                    r = ServiceExecSaveJob.Run(arg);
                    switch (r)
                    {
                        case ServiceExecSaveJob.OK:
                            Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn created" + ConsoleColors.Reset);
                            return;
                        case ServiceExecSaveJob.BAD_ARGS:
                            Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Exec-Savejob <id> | <id>;<id>;<id> | <id>,<id>" + ConsoleColors.Reset);
                            return;
                        case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                            Console.WriteLine(ConsoleColors.Red + "\t This job does not exist" + ConsoleColors.Reset);
                            return;
                    } 
                }

            }
            else if(int.TryParse(args[0], out intType))
            {
                r = ServiceExecSaveJob.Run(args);
                switch (r)
                {
                    case ServiceExecSaveJob.OK:
                        Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn created" + ConsoleColors.Reset);
                        return;
                    case ServiceExecSaveJob.BAD_ARGS:
                        Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Exec-Savejob <id> | <id>;<id>;<id> | <id>,<id>" + ConsoleColors.Reset);
                        return;
                    case ServiceExecSaveJob.JOB_DOES_NOT_EXIST:
                        Console.WriteLine(ConsoleColors.Red + "\t This job does not exist" + ConsoleColors.Reset);
                        return;
                }   
            }
            else
            {
                Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Exec-Savejob <id> | <id>;<id>;<id> | <id>,<id>" + ConsoleColors.Reset);
            }
        }

    }
}