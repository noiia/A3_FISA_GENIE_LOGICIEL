using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AvaloniaApplicationClientDistant.Commandes;
using AvaloniaApplicationClientDistant.Message;
using Newtonsoft.Json;

namespace AvaloniaApplicationClientDistant;

public class Client
{
    private static Client instance;
    private Socket clientSocket;
    private Thread inputThread;
    private IPEndPoint serverEndPoint;

    // Constructeur privé pour empêcher l'instanciation directe
    private Client()
    {
    }

    // Méthode pour obtenir l'instance unique de la classe
    public static Client GetInstance()
    {
        if (instance == null) instance = new Client();
        return instance;
    }

    public Socket ConnectToServer()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
        clientSocket.Connect(serverEndPoint);
        Console.WriteLine("Connected to server.");
        return clientSocket;
    }

    public void ListenToServer()
    {
        while (clientSocket.Connected)
        {
            try
            {
                var bytes = new byte[4];
                var bytesRec = clientSocket.Receive(bytes);
                if (bytesRec == 0) Console.WriteLine("Disconnected from server.");

                var nextMessageLenght = BitConverter.ToInt32(bytes, 0);
                try
                {
                    var dataBytes = new byte[nextMessageLenght];
                    bytesRec = clientSocket.Receive(dataBytes);
                    if (bytesRec == 0) Console.WriteLine("Disconnected from server.");

                    var asciiBytes = Encoding.UTF8.GetString(dataBytes);
                    Console.WriteLine(asciiBytes);
                    var msg = JsonConvert.DeserializeObject<MSG>(asciiBytes);
                    Console.WriteLine("New Message ! ");
                    Console.WriteLine(msg.Message);
                    switch (msg.Message)
                    {
                        case "configFile":
                            var configurationDistant = ConfigurationDistant.GetInstance();
                            configurationDistant.ConfigFile =
                                JsonConvert.DeserializeObject<MSGConfigFile>(asciiBytes).ConfigFile;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }

            Thread.Sleep(500);
        }
    }

    public void Disconnect()
    {
        clientSocket.Shutdown(SocketShutdown.Both);
        clientSocket.Close();
        Console.WriteLine("Disconnected from server.");
    }

    public void SendMessage(CMD cmd)
    {
        var asciiBytes = Encoding.UTF8.GetBytes(cmd.toString());
        var leghtJson = asciiBytes.Length;
        var bytesLenght = BitConverter.GetBytes(leghtJson);
        clientSocket.Send(bytesLenght);
        clientSocket.Send(asciiBytes);
    }

    public void Init()
    {
        try
        {
            clientSocket = ConnectToServer();
            if (clientSocket.Connected)
            {
                Console.WriteLine("Connection successful. Starting listener thread.");
                inputThread = new Thread(ListenToServer);
                inputThread.Start();
                Console.WriteLine("Listener thread started.");
            }
            else
            {
                Console.WriteLine("Failed to connect to the server.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Init: {ex.Message}");
        }
    }
}