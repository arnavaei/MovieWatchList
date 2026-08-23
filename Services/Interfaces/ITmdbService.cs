namespace MovieWatchList.Services.Interfaces;

public interface ITmdbService
{
    Task<string?> GetPosterUrlAsync(string movieTitle, CancellationToken cancellationToken = default);
}
