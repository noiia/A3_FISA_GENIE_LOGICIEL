namespace Job.Services;

public class TrackerBigFileEventArgs(Dictionary<int, List<string>> tooBigFiles) : EventArgs
{
    public Dictionary<int, List<string>> TooBigFiles { get; set; } = tooBigFiles;
}