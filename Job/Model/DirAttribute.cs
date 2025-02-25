namespace Job.Model;

public class DirAttribute
{
    public string DirName { get; set; }
    public List<FileInfo> Files { get; set; }
    public List<DirAttribute> SubFolder { get; set; }
}