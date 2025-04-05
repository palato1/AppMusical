namespace Seraphine.Core.Interface;

public interface INotificator
{
    bool HasNotification();
    string ObterNotificacoes();

    void Add(string message);
    void Add(IEnumerable<string> messages);
}