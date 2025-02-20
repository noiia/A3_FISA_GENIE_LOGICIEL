using System.Globalization;
using System.Resources;
using System.Threading;

namespace Job.Config.i18n
{
    public static class Translation
    {
        private static ResourceManager _translator = new ResourceManager("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);

        public static ResourceManager Translator
        {
            get => _translator;
            private set => _translator = value;
        }

        public static void SelectLanguage(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            // Re-initialize the ResourceManager if needed
            Translator = new ResourceManager("Job.Config.i18n.Resources.Resources", typeof(Translation).Assembly);
        }

        public static string GetString(string key)
        {
            try
            {
                return Translator.GetString(key, Thread.CurrentThread.CurrentUICulture);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., missing resource)
                Console.WriteLine($"Error retrieving translation for key '{key}': {ex.Message}");
                return $"[{key}]"; // Return the key in brackets if not found
            }
        }
    }
}