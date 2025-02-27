namespace Job.Model;

public class FileInfo
{
    public FileInfo(string fileName, string fileHash)
    {
        FileName = fileName;
        FileHash = fileHash;
    }

    public string FileName { get; set; }
    public string FileHash { get; set; }
}