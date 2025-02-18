using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Job.Config.i18n;

public class Translation
{
    private static ResourceManager _translator = new ResourceManager("Config.i18n.Resources", typeof(Program).Assembly);
    public static ResourceManager Translator
    {
        get => _translator;
        private set => _translator = value;
    }

    public static Func<List<string>> SelectLanguage(string language)
    {
        Thread.CurrentThread.CurrentUICulture = language switch
        {
            "en" => new CultureInfo("en"),
            "fr" => new CultureInfo("fr"),
            _ => new CultureInfo("en")
        };
        Translator = new ResourceManager("Config.i18n.Resources", typeof(Program).Assembly);
        return () => new List<string>{"a", "b"};
    }
}