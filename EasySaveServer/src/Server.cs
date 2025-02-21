using System.Net;
using System.Net.Sockets;
using System.Text;

namespace EasySaveServer;

class Server
    {
        
        List<Socket> clients;
        private Socket StartServer()
        {
            // Crée une nouvelle socket TCP/IP
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Définit les points de terminaison locaux et écoute les connexions entrantes
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 11000);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(10);

            log("Server started and listening on port 11000.");
            return serverSocket;
        }

        private Socket AcceptConnection(Socket serverSocket)
        {
            // Accepte une connexion entrante
            Socket clientSocket = serverSocket.Accept();
            clients.Add(clientSocket);
            log("Client connected.");
            log(clientSocket.RemoteEndPoint.ToString());
            return clientSocket;
        }

        private void ListenToClient(Socket clientSocket)
        {
            // Écoute les données du client
            byte[] bytes = new byte[1024];
            int bytesRec = clientSocket.Receive(bytes);
            string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            log("Client: " + data);

            // Répond au client
            string filePath = @"C:\Users\thoma\Documents\Exemple\progress.txt";
            string response = "";
            try
            {
                string content = File.ReadAllText(filePath);
                Console.WriteLine("Contenu du fichier :");
                Console.WriteLine(content);
                response = content;
            }
            catch (Exception ex)
            {
                // Gérer les exceptions possibles (fichier non trouvé, accès refusé, etc.)
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
                response = $"Une erreur s'est produite : {ex.Message}";
            }
            byte[] msg = Encoding.ASCII.GetBytes(response);
            clientSocket.Send(msg);
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
        
        public void Run()
        {
            Socket serverSocket = StartServer();
            while (true)
            {
                Socket clientSocket = AcceptConnection(serverSocket);
                ListenToClient(clientSocket);
                Disconnect(clientSocket);
            }
        }
    }