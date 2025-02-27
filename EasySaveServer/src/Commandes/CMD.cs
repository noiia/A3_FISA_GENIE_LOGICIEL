using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Commandes;

public class CMD
{
    protected string commande;

    public CMD(string commande)
    {
        this.commande = commande;
    }

    public virtual string toString()
    {
        JObject json = new JObject();
        json.Add("commande", commande);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public virtual Task run(MessageList messageList)
    {
        return Task.CompletedTask;
    }

    public string Commande
    {
        get => commande;
        set => commande = value ?? throw new ArgumentNullException(nameof(value));
    }
}