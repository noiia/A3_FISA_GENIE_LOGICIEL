using System.Net;
using System.Net.Sockets;
using Job.Services;

namespace EasySaveServer;

internal class Server
{
    private ClientList clientList;
    private MessageList messageList;

    private Socket StartServer()
    {
        clientList = new ClientList();
        messageList = new MessageList();
        var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
        serverSocket.Bind(localEndPoint);
        serverSocket.Listen(10);
        log("Server started and listening on port 11000.");
        return serverSocket;
    }

    private Client AcceptConnection(Socket serverSocket)
    {
        Console.WriteLine("Waiting for a connection...");
        var clientSocket = serverSocket.Accept();
        Console.WriteLine("New client connected.");
        Console.WriteLine("Ip : " + clientSocket.RemoteEndPoint);
        var uuid = Guid.NewGuid().ToString();
        Console.WriteLine("New uuid : " + uuid);
        var c = new Client(uuid, clientSocket);
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
        var serverSocket = StartServer();
        Console.WriteLine("Server started.");

        var output = new Output(clientList, messageList);
        Console.WriteLine("Output instance created.");


        var inputTasks = new List<Input>();
        Console.WriteLine("Input task list initialized.");

        var configuration = ConfigSingleton.Instance();
        var _ = new SaveJobRepo(configuration, 5);

        while (true)
        {
            Console.WriteLine("Waiting for new client connection...");
            var client = AcceptConnection(serverSocket);
            Console.WriteLine("New client connection accepted.");

            var input = new Input(client, clientList, messageList);
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