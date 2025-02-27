using System.Text;

namespace EasySaveServer;

public class Output
{
    private readonly ClientList clientList;
    private readonly MessageList messageList;
    private readonly Thread sendThread;

    public Output(ClientList clientList, MessageList messageList)
    {
        this.clientList = clientList;
        Console.WriteLine("Client list initialized in Output.");

        this.messageList = messageList;
        Console.WriteLine("Message list initialized in Output.");

        // Initialize and start the thread
        sendThread = new Thread(RunTask);
        sendThread.Start();
    }

    private void RunTask()
    {
        Console.WriteLine("Starting send task...");

        while (true)
        {
            if (messageList.Messages.Count > 0)
            {
                Console.WriteLine("Message found in the list.");

                var msg = Encoding.UTF8.GetBytes(messageList.Messages[0]);
                var leghtJson = msg.Length;
                var bytesLenght = BitConverter.GetBytes(leghtJson);
                Console.WriteLine("Message encoded to bytes.");

                foreach (var client in clientList.Clients)
                {
                    Console.WriteLine("Sending message to client: " + client.uuid);
                    client.socket.Send(bytesLenght);
                    client.socket.Send(msg);
                }

                messageList.Messages.RemoveAt(0);
                Console.WriteLine("Message sent and removed from the list.");
            }

            Thread.Sleep(500);
        }
    }
}