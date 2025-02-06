namespace test;

public class CompleteBackup : Backup
{
    private static CompleteBackup instance;

    private string BackupId;

    private CompleteBackup(string rootDir, string saveDir) : base(rootDir, saveDir) {}

    public static CompleteBackup GetInstance(string rootDir, string saveDir)
    {
        if (instance == null)
        {
            instance = new CompleteBackup(rootDir, saveDir);
        }
        return instance;
    }
    

    
}