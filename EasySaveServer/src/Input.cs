namespace EasySaveServer;

public class Input
{
    public Client client { get; set; }
    
    public Input(Client client, ClientList clientList, MessageList messageList)
    {
        bool shutdown = false;
        while (!shutdown)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = client.socket.Receive(bytes);
        }
    }
}