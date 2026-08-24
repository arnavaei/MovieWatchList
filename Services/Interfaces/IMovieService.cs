using MovieWatchList.Models;

namespace MovieWatchList.Services.Interfaces;

public interface IMovieService
{
    IReadOnlyList<Movie> GetMovies();

    IReadOnlyList<Movie> GetWatchList();

    Task<Movie?> GetMovieByIdAsync(int id);

    Task AddToWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default);

    Task RemoveFromWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default);
}