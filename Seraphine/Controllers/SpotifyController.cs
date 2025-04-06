using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Text;

namespace Seraphine.Controllers
{
    [Route("spotify")]
    public class SpotifyController(IConfiguration _config) : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            var clientId = _config["Spotify:ClientId"];
            var redirectUri = Uri.EscapeDataString(_config["Spotify:RedirectUri"]);
            var scope = Uri.EscapeDataString("user-read-email user-read-private");

            var authUrl = $"https://accounts.spotify.com/authorize" +
                          $"?response_type=code" +
                          $"&client_id={clientId}" +
                          $"&scope={scope}" +
                          $"&redirect_uri={redirectUri}";

            return Redirect(authUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return BadRequest("Código de autorização ausente");
            }

            var clientId = _config["Spotify:ClientId"];
            var clientSecret = _config["Spotify:ClientSecret"];
            var redirectUri = _config["Spotify:RedirectUri"];

            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            // 1. Criar cliente RestSharp
            var options = new RestClientOptions("https://accounts.spotify.com")
            {
                ThrowOnAnyError = false
            };
            var client = new RestClient(options);

            // 2. Montar request de token
            var tokenRequest = new RestRequest("api/token", Method.Post);
            tokenRequest.AddHeader("Authorization", $"Basic {authHeader}");
            tokenRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            tokenRequest.AddParameter("grant_type", "authorization_code");
            tokenRequest.AddParameter("code", code);
            tokenRequest.AddParameter("redirect_uri", redirectUri);

            var tokenResponse = await client.ExecuteAsync(tokenRequest);

            if (!tokenResponse.IsSuccessful)
            {
                return BadRequest($"Erro ao obter token: {tokenResponse.Content}");
            }

            var tokenData = Newtonsoft.Json.JsonConvert.DeserializeObject<SpotifyTokenResponse>(tokenResponse.Content!);
            string accessToken = tokenData.access_token;

            var apiClient = new RestClient("https://api.spotify.com");
            var profileRequest = new RestRequest("v1/me", Method.Get);
            profileRequest.AddHeader("Authorization", $"Bearer {accessToken}");

            var profileResponse = await apiClient.ExecuteAsync(profileRequest);

            if (!profileResponse.IsSuccessful)
            {
                return BadRequest("Erro ao obter perfil do usuário");
            }

            return Content(profileResponse.Content!, "application/json");
        }

        public class SpotifyTokenResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
        }
    }
}
