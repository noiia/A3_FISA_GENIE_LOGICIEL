using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client.Commandes;

public class CMD
{
    protected string command;

    public CMD(string command)
    {
        this.command = command;
    }

    public virtual string toString()
    {
        JObject json = new JObject();
        json.Add("commande", command);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public string Command
    {
        get => command;
        set => command = value ?? throw new ArgumentNullException(nameof(value));
    }
}