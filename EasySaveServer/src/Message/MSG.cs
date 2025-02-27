using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EasySaveServer.Message;

public class MSG
{
    protected string name;

    public MSG(string name)
    {
        this.name = name;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual string toString()
    {
        var json = new JObject();
        json.Add("commande", name);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }

    public virtual void run(MessageList messageList)
    {
    }
}