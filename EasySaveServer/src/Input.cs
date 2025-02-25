using System;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer
{
    public class Input
    {
        public Client client { get; set; }
        public ClientList clientList { get; set; }
        public MessageList messageList { get; set; }
        private Thread receiveThread;
        private volatile bool isRunning;

        public Input(Client client, ClientList clientList, MessageList messageList)
        {
            this.client = client;
            this.clientList = clientList;
            this.messageList = messageList;
            // Initialize and start the thread
            isRunning = true;
            receiveThread = new Thread(new ThreadStart(RunTask));
            receiveThread.Start();
        }

        private void RunTask()
        {
            Console.WriteLine($"[ {client.uuid} ] Starting receive task...");

            while (isRunning)
            {
                try
                {
                    byte[] bytes = new byte[4];
                    Console.WriteLine($"[ {client.uuid} ] Waiting to receive data lenght");
                    int bytesRec = client.socket.Receive(bytes);
                    if (bytesRec == 0)
                    {
                        // Connection closed
                        Console.WriteLine($"[ {client.uuid} ] Connection closed");
                        this.Stop();
                        break;
                    }
                    int nextMessageLenght = BitConverter.ToInt32(bytes, 0);
                    // Lock la comunication
                    Console.WriteLine($"[ {client.uuid} ] Next message legnht : " + nextMessageLenght);
                    try
                    {
                        byte[] dataBytes = new byte[nextMessageLenght];
                        Console.WriteLine($"[ {client.uuid} ] Waiting to receive data lenght");
                        bytesRec = client.socket.Receive(dataBytes);
                        if (bytesRec == 0)
                        {
                            // Connection closed
                            Console.WriteLine($"[ {client.uuid} ] Connection closed");
                            this.Stop();
                            break;
                        }
                        string asciiBytes = Encoding.UTF8.GetString(dataBytes);
                        Console.WriteLine($"[ {client.uuid} ] Message decoded : { asciiBytes }");
                        try
                        {
                            CommandeFromObject commandeFromObject = new CommandeFromObject(asciiBytes);
                            commandeFromObject.init();
                            commandeFromObject.run();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                            this.Stop();
                        }
                    
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                        this.Stop();
                    }
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ {client.uuid} ] Error while receive data: {e.Message}");
                    this.Stop();
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
            foreach (Client c in clientList.Clients)
            {
                Console.WriteLine($"\t[{c.uuid}]");
            }
        }
    }
}