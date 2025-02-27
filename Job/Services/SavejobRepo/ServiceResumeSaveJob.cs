using System.Diagnostics;
using Job.Config.i18n;
using ExecSaveJob;
using Job.Config;
using Job.Services.ExecSaveJob;
using Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Job.Services;

public class ServiceResumeSaveJob
{
    public delegate Backup BackupDelegate(SaveJob saveJob);

    public static (int, string) Run(Configuration configuration, int id)
    {
        int backupId = RealTimeState.GetBackupIdBySaveJobId(id);
        // Console.WriteLine("1");
        ResumeBackup resumeBackup = new ResumeBackup(configuration.GetSaveJob(id));
        // Console.WriteLine("2");
        resumeBackup.Resume(configuration, backupId);
        return (0, "");
    }




    // public static void ContinueSaveJob(Configuration configuration, int backupId)
    // {
    //     List<string> filesToSave;
    //     List<(string, int)> filesToResume;
    //     (filesToSave, filesToResume) = GetFilesForResume(configuration, backupId) ?? (new List<string>(), new List<(string, int)>());
    //     // List<BackupFile> backupFiles = GetFilesAdvancementByBackupId(backupId);
    //
    //     // foreach (BackupFile backupFile in backupFiles)
    //     // {
    //         // string source = backupFile.Source;
    //         // string destination = backupFile.Destination;
    //         // string advancement = backupFile.Advancement;
    //         // FileInfo fileInfo = new FileInfo(source);
    //         // Counters counter = new Counters(1, 1, true);
    //         // counter.FileCount = 1;
    //         // counter.DataCount = fileInfo.Length;
    //         // WriteState("ContinueSaveJob", counter, fileInfo, destination, "statefile.log", "Continue Save Job", backupId.ToString(), double.Parse(advancement));
    // }
    
    
    
    
    
    
    // public static (int, string) Run(Configuration configuration, int? id)
    // {
    //     SaveJob? saveJob = null;
    //     if (id is not null ^ name is not "")
    //     {
    //         if (id is not null)
    //         {
    //             saveJob = configuration.GetSaveJob(id ?? -1);   
    //         }
    //         else
    //         {
    //             saveJob = configuration.GetSaveJob(name ?? string.Empty);
    //         }
    //
    //         LoggerUtility.WriteLog(LoggerUtility.Info, $"Saving : id: {id.ToString()} name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination})");
    //
    //         Stopwatch stopwatch = new Stopwatch();
    //         stopwatch.Start();
    //         
    //         if (saveJob.Type == "full")
    //         {
    //             // CompleteBackup completeBackup = CompleteBackup.GetInstance(saveJob);
    //             CompleteBackup completeBackup = new CompleteBackup(saveJob);
    //             completeBackup.Save();
    //         }
    //         else if (saveJob.Type == "diff")
    //         {
    //             // DifferentialBackup differentialBackup = DifferentialBackup.GetInstance(saveJob);
    //             DifferentialBackup differentialBackup = new DifferentialBackup(saveJob);
    //             differentialBackup.Save();
    //         }
    //         
    //         stopwatch.Stop();
    //         
    //         LoggerUtility.WriteLog(LoggerUtility.Info, $"The savejob took {stopwatch.ElapsedMilliseconds} ms");
    //         LoggerUtility.WriteLog(LoggerUtility.Info, $"Save : id: {id.ToString()}, name : {saveJob.Name} from ({saveJob.Source}) to ({saveJob.Destination}) is save");
    //         return (1, $"{Translation.Translator.GetString("SjExecSuccesfully") ?? String.Empty} - ID : {id.ToString()}");
    //     }
    //     else
    //     {
    //         string returnSentence = $"{Translation.Translator.GetString("BadArgsGiven") ?? String.Empty} - ID : {id.ToString()}";
    //         LoggerUtility.WriteLog(LoggerUtility.Warning, returnSentence);
    //         return (2, returnSentence);
    //     }
    // }

}