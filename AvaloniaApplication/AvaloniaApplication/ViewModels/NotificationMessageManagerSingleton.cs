using Avalonia.Notification;

namespace AvaloniaApplication.ViewModels;

public class NotificationMessageManagerSingleton : INotificationMessageManager
{
    // Instance statique unique de la classe
    private static NotificationMessageManager _instance;

    // Constructeur privé pour empêcher l'instantiation directe
    private NotificationMessageManagerSingleton()
    {
        // Initialisation du gestionnaire de notifications
    }

    // Propriété publique pour accéder à l'instance unique
    public static NotificationMessageManager Instance
    {
        get
        {
            // Crée l'instance si elle n'existe pas encore
            if (_instance == null)
            {
                _instance = new NotificationMessageManager();
            }
            return _instance;
        }
    }

    // Implémentation des méthodes de INotificationMessageManager
    // ...
    public void Queue(INotificationMessage message)
    {
        throw new System.NotImplementedException();
    }

    public void Dismiss(INotificationMessage message)
    {
        throw new System.NotImplementedException();
    }

    public INotificationMessageFactory Factory { get; set; }
    public event NotificationMessageManagerEventHandler? OnMessageQueued;
    public event NotificationMessageManagerEventHandler? OnMessageDismissed;
}