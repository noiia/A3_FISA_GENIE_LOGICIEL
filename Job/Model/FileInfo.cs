namespace Job.Model;

public class FileInfo
{
    public string FileName { get; set; }
    public string FileHash { get; set; }

    public FileInfo(string fileName, string fileHash)
    {
        FileName = fileName;
        FileHash = fileHash;
    }
}