using System;
using System.IO;
using Services;
using Logger;

namespace CLI
{
    public class CommandeAddSaveJob : Commande
    {
        public CommandeAddSaveJob() : base("Add-SaveJob", new string[] { "asj", "a-sj" }) { }

        public override void Action(string[] args)
        {
            LoggerUtility.WriteLog(LoggerUtility.Info, "Add-SaveJob : is call with args : "+string.Join(" ", args));
            int r = ServiceAddSaveJob.Run(args);
            switch (r)
            {
                case ServiceAddSaveJob.OK:
                    Console.WriteLine(ConsoleColors.Green + "\t Save Job has benn created" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.BAD_ARGS:
                    Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Add-Savejob <name> <source> <destination> <type>" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.NAME_ALREADY_USE:
                    Console.WriteLine(ConsoleColors.Red + "\t A save Job already exist with this name (" + args[0] + ")" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.SOURCE_DOES_NOT_EXIST:
                    Console.WriteLine(ConsoleColors.Red + "\t Source directory does not exist ("  + args[1] + " )" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.DESTINANTION_DOES_NOT_EXIST:
                    Console.WriteLine(ConsoleColors.Red + "\t Destination directory does not exist ( " + args[2] + " )" + ConsoleColors.Reset);
                    return;
                case ServiceAddSaveJob.TYPE_DOES_NOT_EXIST:
                    Console.WriteLine(ConsoleColors.Red + "\t Type is invalid does not exist ( " + args[3] + " ) real type is full | diff" + ConsoleColors.Reset);
                    return;

            }
        }

    }
}