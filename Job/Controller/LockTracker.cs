namespace Job.Controller;

public class LockTracker
{
    private readonly Dictionary<int, Dictionary<string, int>> _processStatus = new();
    public event EventHandler<TrackerLockEventArgs> OnTrackerChanged;

    public void AddOrUpdateLockStatus(int id, string businessAppName, int status)
    {
        if (!_processStatus.ContainsKey(id)) _processStatus[id] = new Dictionary<string, int>();

        _processStatus[id][businessAppName] = status;

        OnTrackerChanged?.Invoke(this, new TrackerLockEventArgs(id, businessAppName, status));
    }
}