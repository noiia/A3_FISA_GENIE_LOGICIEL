namespace EasySaveServer;

public class MessageList
{
    List<string> messages;

    public MessageList()
    { 
        messages = new List<string>();
    }

    public List<string> Messages
    {
        get => messages;
        set => messages = value ?? throw new ArgumentNullException(nameof(value));
    }
}