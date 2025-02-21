using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Client
    {
        public static Socket ConnectToServer()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);
            clientSocket.Connect(serverEndPoint);
            Console.WriteLine("Connected to server.");
            return clientSocket;
        }

        public static void ListenToServer(Socket clientSocket)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = clientSocket.Receive(bytes);
            string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            Console.WriteLine("Received from server: " + data);
        }

        public static void Disconnect(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Console.WriteLine("Disconnected from server.");
        }

        public void Run(string[] args)
        {
            while (true)
            {
                Socket clientSocket = ConnectToServer();
        
                // Envoie un message au serveur
                string message = "Hello, Server, send me progress";
                byte[] msg = Encoding.ASCII.GetBytes(message);
                clientSocket.Send(msg);
        
                // Écoute la réponse du serveur
                ListenToServer(clientSocket);
        
                // Déconnecte le client
                Disconnect(clientSocket);
        
                Console.ReadLine();
            }
        }
    }
}