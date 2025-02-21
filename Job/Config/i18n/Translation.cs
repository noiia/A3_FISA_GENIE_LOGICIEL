using System.Globalization;
using System.Resources;

namespace Job.Config.i18n;

public static class Translation
{
    public static ResourceManager Translator { get; private set; } =
        new("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);

    public static void SelectLanguage(string language)
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        Translator = new ResourceManager("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);
    }

    public static string GetString(string key)
    {
        try
        {
            string? s = Translator.GetString(key, Thread.CurrentThread.CurrentUICulture);
            Console.WriteLine(s);
            return s;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving translation for key '{key}': {ex.Message}");
            return $"[{key}]"; // Return the key in brackets if not found
        }
    }
}