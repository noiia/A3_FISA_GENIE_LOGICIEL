using System;
using Config;
//
// public class ConfigSingleton
// {
//     private Configuration _configuration;
//     private static ConfigSingleton instance;
//
//     // Objet de verrouillage pour assurer l'accès thread-safe
//     private static readonly object lockObject = new object();
//
//     // Constructeur privé pour empêcher l'instanciation directe
//     private ConfigSingleton()
//     {
//         _configuration =
//             new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
//                               "\\EasySave\\" + "config.json");
//         // Initialisation de la configuration
//         // Vous pouvez ajouter ici le code pour charger la configuration
//     }
//
//     // Propriété pour accéder à l'instance unique de la classe
//     public static ConfigSingleton Instance
//     {
//         get
//         {
//             // Double-checked locking pour améliorer les performances
//             if (instance == null)
//             {
//                 lock (lockObject)
//                 {
//                     if (instance == null)
//                     {
//                         instance = new ConfigSingleton();
//                     }
//                 }
//             }
//             return instance;
//         }
//     }
//
//     // Exemple de méthode pour obtenir une valeur de configuration
//     public Configuration Configuration
//     {
//         get => _configuration;
//         set => _configuration = value ?? throw new ArgumentNullException(nameof(value));
//     }
// }


public class ConfigSingleton
{
    public Configuration _configuration { get; private set; }
    private static ConfigSingleton instance;
    private ConfigSingleton()
    {
        _configuration = new Configuration(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                           "\\EasySave\\" + "config.json");
    }
    
    public static Configuration Instance()
    {
        if (instance == null)
        {
            instance = new ConfigSingleton();
        }
        return instance._configuration;
    }
}


