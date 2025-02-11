using System;
using System.Diagnostics;
using System.IO;
using CLI.i18n;

using Config;
using Logger;

namespace CLI
{
    public class CommandeSetLogPath : Commande
    {
        public CommandeSetLogPath() : base("Set-LogPath", new string[] { "slp", "s-lp" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();
            
            LoggerUtility.WriteLog(LoggerUtility.Info, $"{language[26]} {string.Join(" ", args)}");
            ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
            {
                FileName = "ExecSaveJob.exe", // Programme à exécuter
                Arguments = string.Join(' ',args),           // Arguments optionnels
                UseShellExecute = true,    // Utiliser le shell Windows (obligatoire pour certaines applications)
                RedirectStandardOutput = true, // Capture la sortie standard
                RedirectStandardError = true,  // Capture les erreurs
                CreateNoWindow = true         // Évite d'afficher une fenêtre
            };
            
            Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
            processServiceAddSaveJob.Start();
            string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
            string error = processServiceAddSaveJob.StandardError.ReadToEnd();
            processServiceAddSaveJob.WaitForExit();
            Console.WriteLine("Output:");
            Console.WriteLine(output);

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
            switch (processServiceAddSaveJob.ExitCode)
            {
                case SetLogPath.OK:
                    Console.WriteLine($"{ConsoleColors.Green} {language[27]} {ConsoleColors.Reset}");
                    return;
                case SetLogPath.NOT_A_DIR:
                    Console.WriteLine($"{ConsoleColors.Red} \t{args[0]} {language[28]} {ConsoleColors.Reset}");
                    return;
                case SetLogPath.BAD_ARGS:
                    Console.WriteLine($"{ConsoleColors.Red} {language[29]} {ConsoleColors.Reset}");
                    return;
            }
        }

    }
}