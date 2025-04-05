using Seraphine.Core.Interface;

namespace Seraphine.Core;

public class Notificator : INotificator
{
    public readonly List<Notification> Notifications = [];

    public bool HasNotification() => Notifications.Count > 0;
    public string ObterNotificacoes() => string.Join(", ", Notifications.Select(x => x.Message));

    public void Add(string message)
    {
        if (Notifications.Any(x => x.Message == message))
        {
            return;
        }

        Notifications.Add(new(message));
    }

    public void Add(IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            Add(message);
        }
    }
}