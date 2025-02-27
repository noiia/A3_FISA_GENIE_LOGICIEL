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


    public string Message
    {
        get => message;
        set => message = value ?? throw new ArgumentNullException(nameof(value));
    }

    public virtual string toString()
    {
        var json = new JObject();
        json.Add("message", message);
        var jsonString = JsonConvert.SerializeObject(json);
        return jsonString;
    }
}