using System;
using System.IO;
using Services;


namespace CLI
{
    public class CommandeExecSaveJob : Commande
    {
        public CommandeExecSaveJob() : base("Exec-SaveJob", new string[] { "execsj", "e-sj" }) { }

        public override void Action(string[] args)
        {
            int r = ServiceExecSaveJob.Run(args);
            switch (r)
            {
                case ServiceAddSaveJob.OK:
                    Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn created" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.BAD_ARGS:
                    Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Add-Savejob <name> <source> <destination>" + ConsoleColors.Reset);
                    return;
            }
        }

    }
}