namespace Seraphine.Core.Interface;

public interface ICredentialUser
{
    Guid IdUsuarioLogado { get; }
    void SetarIdUsuarioLogado(Guid id);
}