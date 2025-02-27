using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Commandes;

public class CMD
{
    protected string command;

    public CMD(string command)
    {
        this.command = command;
    }

    public string Command
    {
        get => command;
        set => command = value ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual string toString()
    {
        var json = new JObject();
        json.Add("commande", command);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}