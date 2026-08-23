using System.Net.Http.Json;
using MovieWatchList.Dtos;

namespace MovieWatchList.Services;

public class TmdbService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseImageUrl = "https://image.tmdb.org/t/p/w500";

    public TmdbService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["TmdbApiKey"]
                  ?? throw new InvalidOperationException("TmdbApiKey is missing from configuration.");
    }

    public async Task<string?> GetPosterUrlAsync(string movieTitle)
    {
        var url = $"https://api.themoviedb.org/3/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(movieTitle)}";

        var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>(url);

        var firstResult = response?.Results.FirstOrDefault();

        if (firstResult?.PosterPath is null)
        {
            return null;
        }

        return $"{BaseImageUrl}{firstResult.PosterPath}";
    }
}