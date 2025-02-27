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

    public virtual string toString()
    {
        JObject json = new JObject();
        json.Add("commande", name);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    
    public virtual void run(MessageList messageList)
    {
        return;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }
}