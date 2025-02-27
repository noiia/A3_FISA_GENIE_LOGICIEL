using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AvaloniaApplicationClientDistant.Message;

public class MSG
{
    protected string message;

    public MSG(string message)
    {
        this.message = message;
    }

    public virtual string toString()
    {
        JObject json = new JObject();
        json.Add("message", message);
        string jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
    

    public string Message
    {
        get => message;
        set => message = value ?? throw new ArgumentNullException(nameof(value));
    }
}