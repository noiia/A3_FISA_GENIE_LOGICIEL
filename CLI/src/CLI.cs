using Job.Services;

namespace CLI;

public class CLI
{
    private static Commande[] _commandes;

    public CLI()
    {
        _commandes = Array.Empty<Commande>();
        _commandes = _commandes.Append(new CommandeHelp()).ToArray();
        _commandes = _commandes.Append(new CommandeListJobs()).ToArray();
        _commandes = _commandes.Append(new CommandeAddSaveJob()).ToArray();
        _commandes = _commandes.Append(new CommandDeleteSaveJob()).ToArray();
        _commandes = _commandes.Append(new CommandeExecSaveJob()).ToArray();
        _commandes = _commandes.Append(new CommandeSetLogPath()).ToArray();
        _commandes = _commandes.Append(new CommandeSetLanguage()).ToArray();
    }

    public void Run(string[] args)
    {
        DisplayEasySave();
        var configuration = ConfigSingleton.Instance();
        //threadpool a 5 threads
        var saveJobRepo = new SaveJobRepo(configuration, 5);
        if (args.Length == 0)
        {
            foreach (var c in _commandes)
                if (c.IsCall("Help"))
                    c.Run(configuration, args);
        }
        else
        {
            foreach (var c in _commandes)
                if (c.IsCall(args[0]))
                    c.Run(configuration, args.Skip(1).ToArray());
        }
    }

    public static void DisplayEasySave()
    {
        Console.Write(ConsoleColors.Cyan);
        Console.WriteLine("===========================================================");
        Console.WriteLine("  ______                      _____                    ");
        Console.WriteLine(" |  ____|                    / ____|                   ");
        Console.WriteLine(" | |__    __ _  ___  _   _  | (___    __ _ __   __ ___ ");
        Console.WriteLine(" |  __|  / _` |/ __|| | | |  \\___ \\  / _` |\\ \\ / // _ \\");
        Console.WriteLine(" | |____| (_| |\\__ \\| |_| |  ____) || (_| | \\ V /|  __/");
        Console.WriteLine(" |______|\\__,_||___/ \\__, | |_____/  \\__,_|  \\_/  \\___|.exe");
        Console.WriteLine("                      __/ |                            ");
        Console.WriteLine("                     |___/                             ");
        Console.WriteLine("===========================================================");
        Console.Write(ConsoleColors.Reset);
    }
}