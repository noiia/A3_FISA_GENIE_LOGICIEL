using Config;
using Services;

namespace CLI;
using System;
using System.Collections.Generic;

    public class CommandDeleteSaveJob : Commande
    {
        public CommandDeleteSaveJob() : base("delete-SaveJob", new string[] { "delete-save-job", "delete-save-jobs", "delete-jobs", "delete-savejobs", "delete-job", "delete-savejob" }) { }

        public override void Action(string[] args)
        {
            int id = 0;
            try
            {
                id = int.Parse(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("L'id doit etre un nombre ("+args[0]+")");
                return;
            }
            int r = ServiceDeleteSaveJob.Run(args);
            switch (r)
            {
                case ServiceAddSaveJob.OK:
                    Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn deleted" + ConsoleColors.Reset);
                    return;
                case ServiceDeleteSaveJob.BAD_ARGS:
                    Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Delete-SaveJob <id>" + ConsoleColors.Reset);
                    return;
                case ServiceDeleteSaveJob.JOB_DOES_NOT_EXIST:
                    Console.WriteLine(ConsoleColors.Red + "\t Save Job does not exist ( " + args[0] + " )" + ConsoleColors.Reset);
                    return;

            }
        }
    }
