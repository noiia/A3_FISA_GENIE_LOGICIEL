using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client;

public class Client
{
    public static Socket ConnectToServer()
    {
        var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        clientSocket.Connect(serverEndPoint);
        Console.WriteLine("Connected to server.");
        return clientSocket;
    }

    public static void ListenToServer(Socket clientSocket)
    {
        var bytes = new byte[1024];
        var bytesRec = clientSocket.Receive(bytes);
        var data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        Console.WriteLine("Received from server: " + data);
    }

    public static void Disconnect(Socket socket)
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
        Console.WriteLine("Disconnected from server.");
    }

    public static void SendMessage (Socket socket, string commande, int id)
    {
        JObject json = new JObject();
        json.Add("commande", commande);
        json.Add("id", id);
        string jsonString = JsonConvert.SerializeObject(json);
        byte[] asciiBytes = Encoding.ASCII.GetBytes(jsonString);
        int leghtJson = asciiBytes.Length;
        byte[] bytesLenght = BitConverter.GetBytes(leghtJson);
        socket.Send(bytesLenght);
        socket.Send(asciiBytes);
    }
    
    public void Run(string[] args)
    {
        while (true)
        {
            var clientSocket = ConnectToServer();

            
            SendMessage(clientSocket, "Test", 5);
            // Envoie un message au serveur
            // string message = "Hello, Server, send me progress";
            // byte[] msg = Encoding.ASCII.GetBytes(message);
            // var byteLenght = 32;
            // var bytes = BitConverter.GetBytes(byteLenght);
            // clientSocket.Send(bytes);

            // Écoute la réponse du serveur
            // ListenToServer(clientSocket);

            // Déconnecte le client
            Console.ReadLine();
            Disconnect(clientSocket);

            Console.ReadLine();
        }
    }
}