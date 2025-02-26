using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading;
using AvaloniaApplicationClientDistant.Commandes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant
{
    public class Client
    {
        private static Client instance;
        private Socket clientSocket;
        private IPEndPoint serverEndPoint;
        private Thread inputThread;

        // Constructeur privé pour empêcher l'instanciation directe
        private Client() { }

        // Méthode pour obtenir l'instance unique de la classe
        public static Client GetInstance()
        {
            if (instance == null)
            {
                instance = new Client();
            }
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
                    byte[] bytes = new byte[4];
                    int bytesRec = clientSocket.Receive(bytes);
                    if (bytesRec == 0)
                    {
                        Console.WriteLine("Disconnected from server.");
                    }

                    int nextMessageLenght = BitConverter.ToInt32(bytes, 0);
                    try
                    {
                        byte[] dataBytes = new byte[nextMessageLenght];
                        bytesRec = clientSocket.Receive(dataBytes);
                        if (bytesRec == 0)
                        {
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
            byte[] asciiBytes = Encoding.UTF8.GetBytes(cmd.toString());
            int leghtJson = asciiBytes.Length;
            byte[] bytesLenght = BitConverter.GetBytes(leghtJson);
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
                    inputThread = new Thread(new ThreadStart(ListenToServer));
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
}
