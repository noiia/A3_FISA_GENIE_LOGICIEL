// using Job.Config.i18n;

namespace CLI;

internal class Program
{
    private static void Main(string[] args)
    {
        var cli = new CLI();
        cli.Run(args);
        // Console.WriteLine(Translation.Translator.GetString("NoSjToPrint"));
    }
}