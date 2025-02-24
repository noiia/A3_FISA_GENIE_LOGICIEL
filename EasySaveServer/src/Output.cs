using System.Text;

namespace EasySaveServer;

public class Output
{
    private ClientList clientList;
    private MessageList messageList;

    public Output(ClientList clientList, MessageList messageList)
    {
        this.clientList = clientList;
        this.messageList = messageList;
    }

    public Task rTask()
    {
        while (true)
        {
            if (messageList.Messages.Count > 0)
            {
                byte[] msg = Encoding.ASCII.GetBytes(messageList.Messages[0]);
                foreach (Client client in clientList.Clients)
                {
                    client.socket.Send(msg);
                }
                messageList.Messages.RemoveAt(0);
            }
            Thread.Sleep(500);
        }
    }
}