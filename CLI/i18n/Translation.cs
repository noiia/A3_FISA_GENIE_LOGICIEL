namespace CLI.i18n;

public class Translation
{
    public static Func<List<string>> SelectLanguage(string language)
    {
        switch (language)
        {
            case "en":
                return en.EnTranslation;
            case "fr":
                return fr.FrTranslation;
            default:
                return en.EnTranslation;
        }
    }
}