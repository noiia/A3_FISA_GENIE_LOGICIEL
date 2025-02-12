using System;
using System.Diagnostics;
using System.IO;
using System.Transactions;
using CLI.i18n;
using Config;
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
            ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
            {
                FileName = "..\\..\\..\\..\\AddSaveJob\\bin\\Debug\\net8.0\\AddSaveJob.exe", // Programme à exécuter
                Arguments = string.Join(' ', args),           // Arguments optionnels
                UseShellExecute = false,    // Utiliser le shell Windows (obligatoire pour certaines applications)
                RedirectStandardOutput = true, // Capture la sortie standard
                RedirectStandardError = true,  // Capture les erreurs
                CreateNoWindow = true         // Évite d'afficher une fenêtre
            };
            Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
            processServiceAddSaveJob.Start();
            string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
            string error = processServiceAddSaveJob.StandardError.ReadToEnd();
            processServiceAddSaveJob.WaitForExit();

            if (!string.IsNullOrWhiteSpace(error))
            {
                Console.WriteLine("Error:");
                Console.WriteLine(error);
            }
            switch (processServiceAddSaveJob.ExitCode)
            {
                case 1:
                    Console.WriteLine($"{ConsoleColors.Green} {language[1]} {ConsoleColors.Reset}");
                    return;
                case 2:
                    Console.WriteLine($"{ConsoleColors.Red} {language[2]} {ConsoleColors.Reset}");
                    return;
                case 3:
                    Console.WriteLine($"{ConsoleColors.Red} {language[3]} ({args[0]}) {ConsoleColors.Reset}");
                    return;
                case 4:
                    Console.WriteLine($"{ConsoleColors.Red} {language[4]} ({args[1]}) {ConsoleColors.Reset}");
                    return;
                case 5:
                    Console.WriteLine($"{ConsoleColors.Red} {language[5]} ({args[2]}) {ConsoleColors.Reset}");
                    return;
                case 6:
                    Console.WriteLine($"{ConsoleColors.Red} {language[6]} ({args[3]}) {language[7]} {ConsoleColors.Reset}");
                    return;

            }
        }

    }
}