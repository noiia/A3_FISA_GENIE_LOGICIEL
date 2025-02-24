namespace Job.Controller;

public class TrackerChangedEventArgs : EventArgs
{
    public int Id { get; }
    public DateTime Timestamp { get; }
    public int ReturnCode { get; }

    public TrackerChangedEventArgs(int id, DateTime timestamp, int returnCode)
    {
        Id = id;
        Timestamp = timestamp;
        ReturnCode = returnCode;
    }
}