using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Client.Commandes;
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
        // var bytes = new byte[1024];
        // var bytesRec = clientSocket.Receive(bytes);
        // var data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
        // Console.WriteLine("Received from server: " + data);
        try
        {
            byte[] bytes = new byte[4];
            int bytesRec = clientSocket.Receive(bytes);
            if (bytesRec == 0)
            {
                // Connection closed
                Console.WriteLine("Disconnected from server.");
            }

            int nextMessageLenght = BitConverter.ToInt32(bytes, 0);
            // Lock la comunication
            try
            {
                byte[] dataBytes = new byte[nextMessageLenght];
                bytesRec = clientSocket.Receive(dataBytes);
                if (bytesRec == 0)
                {
                    // Connection closed
                    Console.WriteLine("Disconnected from server.");
                }

                string asciiBytes = Encoding.UTF8.GetString(dataBytes);
                try
                {
                    Console.WriteLine(asciiBytes);
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
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}");
        }
    }

    public static void Disconnect(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Console.WriteLine("Disconnected from server.");
        }

        public static void SendMessage(Socket socket, CMD cmd)
        {
            byte[] asciiBytes = Encoding.UTF8.GetBytes(cmd.toString());
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


                SendMessage(clientSocket,
                    new CMDAddSaveJob("test", "c:\\Users\\thoma\\Desktop\\test.txt",
                        "c:\\Users\\thoma\\Desktop\\test.txt", "full"));
                ListenToServer(clientSocket);
                Console.ReadLine();
                Disconnect(clientSocket);

                Console.ReadLine();
            }
        }
    }