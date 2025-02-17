using System.Diagnostics;

using Config;
using Config.i18n;

namespace Controller;

public class AddSaveJob
{
    public static string Execute(Configuration configuration, string[] args)
    {
        Func<List<string>> languageFunc = Translation.SelectLanguage(configuration.GetLanguage());
        List<string> language = languageFunc();
        
        ProcessStartInfo serviceAddSaveJob = new ProcessStartInfo
        {
            FileName = "AddSaveJob.exe", 
            Arguments = string.Join(' ', args), 
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

        if (!string.IsNullOrWhiteSpace(error))
        {
            return String.Join("Error: ", error);
        }
        switch (processServiceAddSaveJob.ExitCode)
        {
            case 1:
                return language[1];
            case 2:
                return language[2];
            case 3:
                return String.Join(language[3], args[0]);
            case 4:
                return String.Join(language[4], args[1]);
            case 5:
                return String.Join(language[5], args[2]);
            case 6:
                return String.Join(language[6], args[3], language[7]);
            default:
                return String.Empty;
        }
    }
}