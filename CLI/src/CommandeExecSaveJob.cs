using System;
using System.Diagnostics;
using System.IO;

using Config.i18n;
using Config;
using Logger;
using Services;


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
            if(args[0].Contains(";"))
            {
                string[] listIds = args[0].Split(';');
                foreach (var id in listIds)
                {
                    ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
                    {
                        FileName = "ExecSaveJob.exe", // Programme à exécuter
                        Arguments = id,           // Arguments optionnels
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
                    Console.WriteLine("Output:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(error);
                    }
                    switch (processServiceAddSaveJob.ExitCode)
                    {
                        case ReturnCodes.OK:
                            Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                            return;
                        case ReturnCodes.BAD_ARGS:
                            Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                            return;
                        case ReturnCodes.JOB_DOES_NOT_EXIST:
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
                    ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
                    {
                        FileName = "ExecSaveJob.exe", // Programme à exécuter
                        Arguments = i.ToString(),           // Arguments optionnels
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
                    Console.WriteLine("Output:");
                    Console.WriteLine(output);

                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        Console.WriteLine("Error:");
                        Console.WriteLine(error);
                    }
                    switch (processServiceAddSaveJob.ExitCode)
                    {
                        case ReturnCodes.OK:
                            Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                            return;
                        case ReturnCodes.BAD_ARGS:
                            Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                            return;
                        case ReturnCodes.JOB_DOES_NOT_EXIST:
                            Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                            return;
                    } 
                }

            }
            else if(int.TryParse(args[0], out intType))
            {
                ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
                {
                    FileName = "ExecSaveJob.exe", // Programme à exécuter
                    Arguments = string.Join(' ',args),           // Arguments optionnels
                    UseShellExecute = false,    // Utiliser le shell Windows (obligatoire pour certaines applications)
                    RedirectStandardOutput = true, // Capture la sortie standard
                    RedirectStandardError = true,  // Capture les erreurs
                    CreateNoWindow = true         // Évite d'afficher une fenêtre
                };
            
                Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
                processServiceAddSaveJob.Start();
                processServiceAddSaveJob.WaitForExit();
                string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
                string error = processServiceAddSaveJob.StandardError.ReadToEnd();
                Console.WriteLine("Output:");
                Console.WriteLine(output);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(error);
                }
                switch (processServiceAddSaveJob.ExitCode)
                {
                    case ReturnCodes.OK:
                        Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                        return;
                    case ReturnCodes.BAD_ARGS:
                        Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                        return;
                    case ReturnCodes.JOB_DOES_NOT_EXIST:
                        Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                        return;
                }   
            }
            else
            {
                ProcessStartInfo serviceExecSaveJob = new ProcessStartInfo
                {
                    FileName = "ExecSaveJob.exe",
                    Arguments = string.Join(' ', args),
                    UseShellExecute = false,  // Changed from true to false
                    RedirectStandardOutput = true, // Added to capture output
                    RedirectStandardError = true,  // Added to capture errors
                    CreateNoWindow = true
                };
                Process processServiceExecSaveJob = new Process { StartInfo = serviceExecSaveJob };
                processServiceExecSaveJob.Start();
                processServiceExecSaveJob.WaitForExit();
                string output = processServiceExecSaveJob.StandardOutput.ReadToEnd();
                string error = processServiceExecSaveJob.StandardError.ReadToEnd();
                Console.WriteLine("Output:");
                Console.WriteLine(output);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    Console.WriteLine("Error:");
                    Console.WriteLine(error);
                }
                switch (processServiceExecSaveJob.ExitCode)
                {
                    case ReturnCodes.OK:
                        Console.WriteLine($"{ConsoleColors.Green} {language[14]} {ConsoleColors.Reset}");
                        return;
                    case ReturnCodes.BAD_ARGS:
                        Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
                        return;
                    case ReturnCodes.JOB_DOES_NOT_EXIST:
                        Console.WriteLine($"{ConsoleColors.Red} {language[16]} {ConsoleColors.Reset}");
                        return;
                }   
            }
        }

    }
}