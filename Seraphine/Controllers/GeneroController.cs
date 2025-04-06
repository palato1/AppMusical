using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Seraphine.ServiceInterface;

namespace Seraphine.Controllers
{
    [Route("genero")]
    public class GeneroController(ISpotifyService _spotifyService) : Controller
    {
        [HttpGet("genres")]
        public async Task<IActionResult> GetGenres()
        {
            var accessToken = await _spotifyService.GetAccessToken();

            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized("Token de acesso inválido.");
            }

            var client = new RestClient("https://api.spotify.com");
            var request = new RestRequest("v1/recommendations/available-genre-seeds", Method.Get);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                return BadRequest($"({response.StatusCode}) Erro ao buscar gêneros: {response.Content}");

            return Content(response.Content!, "application/json");
        }

    }
}
