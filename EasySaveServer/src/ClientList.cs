namespace EasySaveServer;

public class ClientList
{
    private List<Client> clients;

    public ClientList()
    {
        clients = new List<Client>();
    }

    public List<Client> Clients
    {
        get => clients;
        set => clients = value ?? throw new ArgumentNullException(nameof(value));
    }
}