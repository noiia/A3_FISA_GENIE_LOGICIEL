namespace Job.Controller;

public class ExecutionTracker
{
    public Dictionary<int, Dictionary<DateTime, int>> EndGetter { get; private set; } = new();

    public event EventHandler<TrackerChangedEventArgs> OnTrackerChanged;

    public void AddOrUpdateExecution(int id, DateTime timestamp, int returnCode)
    {
        if (!EndGetter.ContainsKey(id)) EndGetter[id] = new Dictionary<DateTime, int>();

        EndGetter[id][timestamp] = returnCode;

        OnTrackerChanged?.Invoke(this, new TrackerChangedEventArgs(id, timestamp, returnCode));
    }
}