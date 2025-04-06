using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using Seraphine.Core;
using Seraphine.Core.Interface;
using Seraphine.ServiceInterface;
using System.Text;

namespace Seraphine.Service
{
    public class SpotifyService(IOptions<SpotifySettings> spotifyOptions, INotificator _notificator) : ISpotifyService
    {
        private readonly SpotifySettings _settings = spotifyOptions.Value;

        public async Task<string> GetAccessToken()
        {
            var (IsSuccess, TokenJson, ErrorMessage) = await GetRefreshedAccessTokenAsync();

            if (!IsSuccess || 
                string.IsNullOrWhiteSpace(TokenJson))
            {
                _notificator.Add(ErrorMessage ?? "Erro ao buscar Access Token");
                return string.Empty;
            }

            var token = JsonConvert.DeserializeObject<SpotifyTokenResponse>(TokenJson);
            return token!.AccessToken;
        }

        private async Task<(bool IsSuccess, string? TokenJson, string? ErrorMessage)> GetRefreshedAccessTokenAsync()
        {
            var clientId = _settings.ClientId;
            var clientSecret = _settings.ClientSecret;
            var refreshToken = _settings.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
            {
                return (false, null, "Refresh token não configurado.");
            }

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            var options = new RestClientOptions("https://accounts.spotify.com");
            var client = new RestClient(options);

            var request = new RestRequest("api/token", Method.Post);
            request.AddHeader("Authorization", $"Basic {authHeader}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", refreshToken);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                return (false, null, $"Erro ao atualizar token: {response.Content}");
            }

            return (true, response.Content, null);
        }
    }
}
