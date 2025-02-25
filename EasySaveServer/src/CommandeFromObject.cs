using Newtonsoft.Json;

namespace EasySaveServer;

public class CommandeFromObject
{
    private string asciiMessage;
    private string commande;

    public CommandeFromObject(string asciiMessage)
    {
        this.asciiMessage = asciiMessage;
    }

    public void init()
    {
        var message = JsonConvert.DeserializeObject<dynamic>(asciiMessage);
        if (message.commande)
        {
            commande = message.commande;
            Console.WriteLine($"Commande received : {commande}");
        }
        else
        {
            throw new Exception("No commande recived");
        }
    }

    public void run()
    {
        
    }
}