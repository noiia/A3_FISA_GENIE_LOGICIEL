namespace Job.Config;

public class SaveJob
{
    public SaveJob( int id, string name, string source, string destination, DateTime lastSave, DateTime created, string status, string type)
    {
        Source = source;
        Destination = destination;
        Id = id;
        Name = name;
        LastSave = lastSave;
        Created = created;
        Status = status;
        Type = type;
    }

    public string Source { get; }

    public string Destination { get; }

    public int Id { get; }

    public string Name { get; }

    public DateTime LastSave { get; set; }

    public DateTime Created { get; }

    public string Status { get; set; }
    public string Type { get; }
}