using MovieWatchList.Models;

namespace MovieWatchList.Services.Interfaces;

public interface ISeriesService
{
    IReadOnlyList<Series> GetSeries();

    IReadOnlyList<Series> GetRealityShows();

    IReadOnlyList<Series> GetWatchList();

    Task<Series?> GetSeriesByIdAsync(int id);

    Task AddToWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default);

    Task RemoveFromWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default);
}
