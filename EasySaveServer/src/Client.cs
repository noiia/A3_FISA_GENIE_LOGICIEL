using System.Net.Sockets;

namespace EasySaveServer;

public class Client
{
    public Client(string uuid, Socket socket)
    {
        this.uuid = uuid;
        this.socket = socket;
    }

    public string uuid { get; set; }
    public Socket socket { get; set; }
}