namespace Job.Services;

public class BigFileTracker
{
    private readonly Dictionary<int, List<string>> _saveJobImportance = new();
    public event EventHandler<TrackerBigFileEventArgs> OnTrackerChanged;

    public void AddOrUpdateBigFile(int id, List<string> tooBigFile)
    {
        if (!_saveJobImportance.ContainsKey(id)) _saveJobImportance[id] = new List<string>();
        ;

        _saveJobImportance[id] = tooBigFile;
        OnTrackerChanged?.Invoke(this,
            new TrackerBigFileEventArgs(new Dictionary<int, List<string>> { { id, tooBigFile } }));
    }
}