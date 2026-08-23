using MovieWatchList.Models;

namespace MovieWatchList.Services.Interfaces;

public interface IMovieService
{
    IReadOnlyList<Movie> GetMovies();
    Movie? GetMovieById(int id);
    IReadOnlyList<Movie> GetWatchList();
    Task AddToWatchListAsync(Movie movie, CancellationToken cancellationToken = default);
    Task RemoveFromWatchListAsync(Movie movie, CancellationToken cancellationToken = default);
}

