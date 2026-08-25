namespace MovieWatchList.Services.Interfaces;

public interface ITmdbService
{
    Task<string?> GetPosterUrlAsync(string movieTitle, CancellationToken cancellationToken = default);
    Task<string?> GetTvPosterUrlAsync(string seriesTitle, CancellationToken cancellationToken = default);
}
