using System.Text;

namespace EasySaveServer;

public class Input
{
    private readonly Thread receiveThread;
    private volatile bool isRunning;

    public Input(Client client, ClientList clientList, MessageList messageList)
    {
        this.client = client;
        this.clientList = clientList;
        this.messageList = messageList;
        // Initialize and start the thread
        isRunning = true;
        receiveThread = new Thread(RunTask);
        receiveThread.Start();
    }

    public Client client { get; set; }
    public ClientList clientList { get; set; }
    public MessageList messageList { get; set; }

    private void RunTask()
    {
        Console.WriteLine($"[ {client.uuid} ] Starting receive task...");

        while (isRunning)
        {
            try
            {
                var bytes = new byte[4];
                Console.WriteLine($"[ {client.uuid} ] Waiting to receive data lenght");
                var bytesRec = client.socket.Receive(bytes);
                if (bytesRec == 0)
                {
                    // Connection closed
                    Console.WriteLine($"[ {client.uuid} ] Connection closed");
                    Stop();
                    break;
                }

                var nextMessageLenght = BitConverter.ToInt32(bytes, 0);
                // Lock la comunication
                Console.WriteLine($"[ {client.uuid} ] Next message legnht : " + nextMessageLenght);
                try
                {
                    var dataBytes = new byte[nextMessageLenght];
                    Console.WriteLine($"[ {client.uuid} ] Waiting to receive data lenght");
                    bytesRec = client.socket.Receive(dataBytes);
                    if (bytesRec == 0)
                    {
                        // Connection closed
                        Console.WriteLine($"[ {client.uuid} ] Connection closed");
                        Stop();
                        break;
                    }

                    var asciiBytes = Encoding.UTF8.GetString(dataBytes);
                    Console.WriteLine($"[ {client.uuid} ] Message decoded : {asciiBytes}");
                    try
                    {
                        var commandeFromObject = new CommandeFromObject(asciiBytes, messageList);
                        commandeFromObject.run();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                        Stop();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                    Stop();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                Stop();
            }

            Thread.Sleep(500);
        }

        Console.WriteLine($"[ {client.uuid} ] Receive task stopped");
    }

    public void Stop()
    {
        isRunning = false;
        removeFromClientList();
        receiveThread.Join();
    }

    public void removeFromClientList()
    {
        clientList.Clients.Remove(client);
        Console.WriteLine($"[ {client.uuid} ] Client removed from the list");
        Console.WriteLine("ClientList :");
        foreach (var c in clientList.Clients) Console.WriteLine($"\t[{c.uuid}]");
    }
}