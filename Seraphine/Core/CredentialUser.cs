using Seraphine.Core.Interface;

namespace Seraphine.Core;

public class CredentialUser : ICredentialUser
{
    public Guid IdUsuarioLogado { get; private set; }

    public void SetarIdUsuarioLogado(Guid id)
    {
        IdUsuarioLogado = id;
    }
}