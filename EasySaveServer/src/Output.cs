using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySaveServer
{
    public class Output
    {
        private ClientList clientList;
        private MessageList messageList;
        private Thread sendThread;

        public Output(ClientList clientList, MessageList messageList)
        {
            this.clientList = clientList;
            Console.WriteLine("Client list initialized in Output.");

            this.messageList = messageList;
            Console.WriteLine("Message list initialized in Output.");

            // Initialize and start the thread
            sendThread = new Thread(new ThreadStart(RunTask));
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

                    byte[] msg = Encoding.UTF8.GetBytes(messageList.Messages[0]);
                    Console.WriteLine("Message encoded to bytes.");

                    foreach (Client client in clientList.Clients)
                    {
                        Console.WriteLine("Sending message to client: " + client.uuid);
                        client.socket.Send(msg);
                    }

                    messageList.Messages.RemoveAt(0);
                    Console.WriteLine("Message sent and removed from the list.");
                }
                Thread.Sleep(500);
            }
        }
    }
}
