using System.Diagnostics;


using Config;
using Config.i18n;
using Logger;
using Services;

namespace Controller;

public class ExecuteSaveJob
{
    public static string Execute(Configuration configuration, string[] args)
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
                    FileName = "ExecSaveJob.exe", 
                    Arguments = id,           
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true, 
                    CreateNoWindow = true         
                };
        
                Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
                processServiceAddSaveJob.Start();
                string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
                string error = processServiceAddSaveJob.StandardError.ReadToEnd();
                processServiceAddSaveJob.WaitForExit();
                
                String.Join("Output:", output);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    String.Join("Error:", error);
                }
                
                switch (processServiceAddSaveJob.ExitCode)
                {
                    case ReturnCodes.OK:
                        return language[14];
                    case ReturnCodes.BAD_ARGS:
                        return language[15];
                    case ReturnCodes.JOB_DOES_NOT_EXIST:
                        return language[16];
                    default:
                        return String.Empty;
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
                    FileName = "ExecSaveJob.exe", 
                    Arguments = i.ToString(),     
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true, 
                    CreateNoWindow = true         
                };
        
                Process processServiceAddSaveJob = new Process { StartInfo = serviceAddSaveJob };
                processServiceAddSaveJob.Start();
                string output = processServiceAddSaveJob.StandardOutput.ReadToEnd();
                string error = processServiceAddSaveJob.StandardError.ReadToEnd();
                processServiceAddSaveJob.WaitForExit();
                
                String.Join("Output:", output);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    String.Join("Error:", error);
                }
                
                switch (processServiceAddSaveJob.ExitCode)
                {
                    case ReturnCodes.OK:
                        return language[14];
                    case ReturnCodes.BAD_ARGS:
                        return language[15];
                    case ReturnCodes.JOB_DOES_NOT_EXIST:
                        return language[16];
                    default:
                        return String.Empty;
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
            
            String.Join("Output:", output);
            if (!string.IsNullOrWhiteSpace(error))
            {
                String.Join("Error:", error);
            }
            
            switch (processServiceAddSaveJob.ExitCode)
            {
                case ReturnCodes.OK:
                    return language[14];
                case ReturnCodes.BAD_ARGS:
                    return language[15];
                case ReturnCodes.JOB_DOES_NOT_EXIST:
                    return language[16];
                default:
                    return String.Empty;
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
            
            String.Join("Output:", output);
            if (!string.IsNullOrWhiteSpace(error))
            {
                String.Join("Error:", error);
            }
            
            switch (processServiceExecSaveJob.ExitCode)
            {
                case ReturnCodes.OK:
                    return language[14];
                case ReturnCodes.BAD_ARGS:
                    return language[15];
                case ReturnCodes.JOB_DOES_NOT_EXIST:
                    return language[16];
                default:
                    return String.Empty;
            }   
        }
        return String.Empty;
    }
}