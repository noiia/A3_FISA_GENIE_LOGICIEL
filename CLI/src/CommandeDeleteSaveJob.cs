using System;
using System.Collections.Generic;
using System.Diagnostics;

using Config.i18n;
using Config;
using Job.Config;
using Job.Config.i18n;
using Logger;

namespace CLI;


    public class CommandDeleteSaveJob : Commande
    {
        public CommandDeleteSaveJob() : base("delete-SaveJob", new string[] { "delete-save-job", "delete-save-jobs", "delete-jobs", "delete-savejobs", "delete-job", "delete-savejob" }) { }

        public override void Action(Configuration configuration, string[] args)
        {
            Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
            List<string> language = languageFunc();

            LoggerUtility.WriteLog(LoggerUtility.Info, language[8] + string.Join(" ", args));
            
            ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
            {
                FileName = "DeleteSaveJob.exe", // Programme à exécuter
                Arguments = args[0],           // Arguments optionnels
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
                    Console.WriteLine($"{ConsoleColors.Green} {language[10]} {ConsoleColors.Reset}");
                    return;
                case 2:
                    Console.WriteLine($"{ConsoleColors.Red} {language[11]} {ConsoleColors.Reset}");
                    return;
                case 3:
                    Console.WriteLine($"{ConsoleColors.Red} {language[12]}{args[0]}) {ConsoleColors.Reset}");
                    return;
            }
        }
    }
