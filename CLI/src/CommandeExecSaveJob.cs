using System;
using System.Diagnostics;
using System.IO;

using CLI.i18n;
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
                        FileName = "..\\..\\..\\..\\ExecSaveJob\\bin\\Debug\\net8.0\\ExecSaveJob.exe", // Programme à exécuter
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
                        FileName = "..\\..\\..\\..\\ExecSaveJob\\bin\\Debug\\net8.0\\ExecSaveJob.exe", // Programme à exécuter
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
                    FileName = "..\\..\\..\\..\\ExecSaveJob\\bin\\Debug\\net8.0\\ExecSaveJob.exe", // Programme à exécuter
                    Arguments = string.Join(' ',args),           // Arguments optionnels
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
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
            else
            {
                Console.WriteLine($"{ConsoleColors.Red} {language[15]} {ConsoleColors.Reset}");
            }
        }

    }
}