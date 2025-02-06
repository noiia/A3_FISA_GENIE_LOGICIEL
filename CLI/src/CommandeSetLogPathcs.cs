using System;
using System.IO;
using Services;
using Logger;

namespace CLI
{
    public class CommandeSetLogPath : Commande
    {
        public CommandeSetLogPath() : base("Set-LogPath", new string[] { "slp", "s-lp" }) { }

        public override void Action(string[] args)
        {
            LoggerUtility.WriteLog(LoggerUtility.Info, "Set-LogPath : is call with args : "+string.Join(" ", args));
            int r = ServiceAddSaveJob.Run(args);
            switch (r)
            {
                case ServiceSetLogPath.OK:
                    Console.WriteLine(ConsoleColors.Green + "\t LogPath has been updated" + ConsoleColors.Reset);
                    return;
                case ServiceSetLogPath.NOT_A_DIR:
                    Console.WriteLine(ConsoleColors.Red + "\t" + args[0] + "is not a valid path" + ConsoleColors.Reset);
                    return;
                case ServiceSetLogPath.BAD_ARGS:
                    Console.WriteLine(ConsoleColors.Red + "\t Bad arguments, require Set-LogPath <logPath>" + ConsoleColors.Reset);
                    return;

            }
        }

    }
}