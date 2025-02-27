namespace Job.Controller;

public class TrackerLockEventArgs(int id, string businessAppName, int status) : EventArgs
{
    public int Id { get; set; } = id;
    public string BusinessAppName { get; set; } = businessAppName;
    public int Status {get; set;} = status;
}