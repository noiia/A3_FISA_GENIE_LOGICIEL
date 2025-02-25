namespace Job.Controller;

public class ExecutionTracker
{
    public event EventHandler<TrackerChangedEventArgs> OnTrackerChanged;

    private Dictionary<int, Dictionary<DateTime, int>> _endGetter = new();

    public Dictionary<int, Dictionary<DateTime, int>> EndGetter
    {
        get => _endGetter;
        private set => _endGetter = value;
    }

    public void AddOrUpdateExecution(int id, DateTime timestamp, int returnCode)
    {
        if (!_endGetter.ContainsKey(id))
        {
            _endGetter[id] = new Dictionary<DateTime, int>();
        }

        _endGetter[id][timestamp] = returnCode;

        OnTrackerChanged?.Invoke(this, new TrackerChangedEventArgs(id, timestamp, returnCode));
    }
}