using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveServer;

class Server
    {
        ClientList clientList;
        MessageList messageList;
        private Socket StartServer()
        {
            clientList = new ClientList();
            messageList = new MessageList();
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(10);
            log("Server started and listening on port 11000.");
            return serverSocket;
        }

        private Client  AcceptConnection(Socket serverSocket)
        {
            Console.WriteLine("Waiting for a connection...");
            Socket clientSocket = serverSocket.Accept();
            Console.WriteLine("New client connected.");
            Console.WriteLine("Ip : " + clientSocket.RemoteEndPoint.ToString());
            string uuid = Guid.NewGuid().ToString();
            Console.WriteLine("New uuid : " + uuid);
            Client c = new Client(uuid, clientSocket);
            clientList.Clients.Add(c);
            Console.WriteLine("Aded to client list.");
            return c;
        }

        private void log(string message)
        {
            // Enregistre un message dans la console
            Console.WriteLine(message);
        }

        private void Disconnect(Socket socket)
        {
            // Ferme la connexion socket
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            log("Client disconnected.");
        }
        
        public Task Run()
        {
            Console.WriteLine("Starting server...");
            Socket serverSocket = StartServer();
            Console.WriteLine("Server started.");

            Output output = new Output(clientList, messageList);
            Console.WriteLine("Output instance created.");
            

            List<Input> inputTasks = new List<Input>();
            Console.WriteLine("Input task list initialized.");

            while (true)
            {
                Console.WriteLine("Waiting for new client connection...");
                Client client = AcceptConnection(serverSocket);
                Console.WriteLine("New client connection accepted.");

                Input input = new Input(client, clientList, messageList);
                Console.WriteLine("Input instance created for new client.");
                

                inputTasks.Add(input);
                Console.WriteLine("Input task added to the list.");

                // ListenToClient(clientSocket);
                // Disconnect(clientSocket);
            }

            Console.WriteLine("Waiting for output task to complete...");
            Console.WriteLine("Output task completed.");
        }

    }