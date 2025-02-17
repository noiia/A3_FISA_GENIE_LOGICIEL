using System.Diagnostics;

using Config;
using Config.i18n;
using Logger;

namespace Controller;

public class DeleteSaveJob
{
    public static string Execute(Configuration configuration, string[] args)
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
                return language[10];
            case 2:
                return language[11];
            case 3:
                return String.Join(language[12], args[0]);
            default:
                return String.Empty;
        }
    }
}