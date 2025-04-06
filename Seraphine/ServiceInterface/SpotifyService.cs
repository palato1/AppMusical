namespace Seraphine.ServiceInterface
{
    public interface ISpotifyService
    {
        Task<string> GetAccessToken();
    }
}
