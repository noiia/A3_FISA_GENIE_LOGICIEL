﻿using Job.Config;

namespace Job.Services.ExecSaveJob;

public class DifferentialBackup : Backup
{
    // private static DifferentialBackup instance;

    public DifferentialBackup(SaveJob saveJob) : base(saveJob)
    {
    }

    // public static DifferentialBackup GetInstance(SaveJob saveJob)
    // {
    //     if (instance == null)
    //     {
    //         instance = new DifferentialBackup(saveJob);
    //     }
    //     
    //     return instance;
    // }

    public override List<string> GetFiles(string RootDir, List<string> Files)
    {
        foreach (var File in Directory.GetFiles(RootDir))
            if (isArchived(File))
                Files.Add(File);

        foreach (var Dir in Directory.GetDirectories(RootDir)) GetFiles(Dir, Files);

        return Files;
    }


    protected bool isArchived(string path)
    {
        bool AreFilesEqual(string File1, string File2)
        {
            if (File1 == File2) return true;

            if (!File.Exists(File1) || !File.Exists(File2)) return false;

            var FileInfo1 = new FileInfo(File1);
            var FileInfo2 = new FileInfo(File2);

            if (FileInfo1.Length != FileInfo2.Length) return false;

            using (var fs1 = File.OpenRead(File1))
            using (var fs2 = File.OpenRead(File2))
            {
                int File1Byte;
                int File2Byte;

                do
                {
                    File1Byte = fs1.ReadByte();
                    File2Byte = fs2.ReadByte();
                } while (File1Byte == File2Byte && File1Byte != -1);

                return File1Byte - File2Byte == 0;
            }
        }

        var lastBackupNumber = getLastBackupNumber(SavesDir);
        // Console.WriteLine(lastBackupNumber);
        var verify = path.Replace(RootDir, SaveDir)
            .Replace((lastBackupNumber + 1).ToString(), lastBackupNumber.ToString());
        // Console.WriteLine(verify);


        var fileInfo = new FileInfo(path);

        if ((fileInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive)
            // Console.WriteLine($"{ConsoleColor.Blue} Le bit d'archive est défini. ");
            return true;

        // Console.WriteLine($"{ConsoleColor.Red}Le bit d'archive n'est pas défini -------------------------------------------------------------.");
        return false;


        if (AreFilesEqual(path, verify))
        {
            Console.WriteLine($"Same file {path} {verify}");
            return true;
        }
        else
        {
            Console.WriteLine($"Different file {path} {verify}");
            return false;
        }


        // FileAttributes attributes = File.GetAttributes(path);
        // Console.WriteLine($"aaa {path} {attributes} {(attributes & FileAttributes.Archive) == FileAttributes.Archive}");
        // return (attributes & FileAttributes.Archive) == FileAttributes.Archive;
    }
}