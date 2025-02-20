using Config;
using Job.Config;

namespace ExecSaveJob;

public class CompleteBackup : Backup
{
    private static CompleteBackup instance;

    private string BackupId;

    public CompleteBackup(SaveJob saveJob) : base(saveJob) {}

    // public CompleteBackup GetInstance(SaveJob saveJob)
    // {
    //     // if (instance == null)
    //     // {
    //     //     instance = new CompleteBackup(saveJob);
    //     // }
    //     //
    //     // return instance;
    //     return new CompleteBackup(SaveJob);
    // }
    

    
}