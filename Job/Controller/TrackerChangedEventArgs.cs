namespace Job.Controller;

public class TrackerChangedEventArgs(int id, DateTime timestamp, int returnCode) : EventArgs
{
    public int Id { get; } = id;
    public DateTime Timestamp { get; } = timestamp;
    public int ReturnCode { get; } = returnCode;
}