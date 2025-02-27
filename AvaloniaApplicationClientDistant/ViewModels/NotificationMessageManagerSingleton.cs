using System;
using System.Linq.Expressions;
using Avalonia.Notification;

namespace AvaloniaApplicationClientDistant.ViewModels;

public class NotificationMessageManagerSingleton : INotificationMessageManager
{
    // Instance statique unique de la classe
    private static NotificationMessageManager _instance;

    private NotificationMessageManagerSingleton()
    { }

    public static NotificationMessageManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new NotificationMessageManager();
            }
            return _instance;
        }
    }
    
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

    public static void GenerateNotification(INotificationMessageManager manager, int returnCode, string message)
    {
        string color;
        string foregroundColor;
        string type;
        switch (returnCode)
        {
            case 1 :
                color = NotifColors.green;
                type = NotifColors.INFO;
                foregroundColor = NotifColors.black;
                break;
            
            case 2 :
                color = NotifColors.yellow;
                type = NotifColors.WARNING;
                foregroundColor = NotifColors.black;
                break;
            case 3 :
                color = NotifColors.red;
                type = NotifColors.ERROR;
                foregroundColor = NotifColors.black;
                break;
            default:
                color = NotifColors.blue;
                type = "No type";
                foregroundColor = NotifColors.black;
                break;
        }
        manager
            .CreateMessage()
            .Accent("#1751C3")
            .Animates(true)
            .Background(color)
            .Foreground(foregroundColor)
            .HasBadge(type)
            .HasMessage(message)
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();  
    }
}