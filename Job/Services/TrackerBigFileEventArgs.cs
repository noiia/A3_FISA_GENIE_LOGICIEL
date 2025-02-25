namespace Job.Services;

public class TrackerBigFileEventArgs(int id, List<string> tooBigFiles) : EventArgs
{
    public int Id { get; set; } = id;
    public List<string> TooBigFiles { get; set; } = tooBigFiles;
}