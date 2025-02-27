using Job.Config;

namespace CLI;

public class Commande
{
    private readonly string[] _CommandeAlias;
    private readonly string _CommandeName;

    public Commande(string commandeName, string[] commandeAlias)
    {
        _CommandeName = commandeName;
        _CommandeAlias = commandeAlias;
    }

    public void Run(Configuration config, string[] args)
    {
        try
        {
            Action(config, args);
        }
        catch (Exception e)
        {
            Action(args);
        }
    }

    public bool IsCall(string commandeName)
    {
        if (commandeName.Equals(_CommandeName, StringComparison.OrdinalIgnoreCase)) return true;

        foreach (var alias in _CommandeAlias)
            if (commandeName.Equals(_CommandeName, StringComparison.OrdinalIgnoreCase))
                return true;

        return false;
    }

    public virtual Task Action(string[] args)
    {
        throw new NotImplementedException();
    }

    public virtual void Action(Configuration config, string[] args)
    {
        throw new NotImplementedException();
    }
}