namespace EasySave
{
    public static class ConsoleColors
    {
        public const string Reset = "\x1b[0m";   // Réinitialisation des couleurs
        public const string Bold = "\x1b[1m";    // Texte en gras
        public const string Underline = "\x1b[4m"; // Souligné

        // Couleurs de texte
        public const string Black = "\x1b[30m";
        public const string Red = "\x1b[31m";
        public const string Green = "\x1b[32m";
        public const string Yellow = "\x1b[33m";
        public const string Blue = "\x1b[34m";
        public const string Magenta = "\x1b[35m";
        public const string Cyan = "\x1b[36m";
        public const string White = "\x1b[37m";

        // Couleurs de fond
        public const string BgBlack = "\x1b[40m";
        public const string BgRed = "\x1b[41m";
        public const string BgGreen = "\x1b[42m";
        public const string BgYellow = "\x1b[43m";
        public const string BgBlue = "\x1b[44m";
        public const string BgMagenta = "\x1b[45m";
        public const string BgCyan = "\x1b[46m";
        public const string BgWhite = "\x1b[47m";
    }
}