using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;

namespace MovieWatchList.Services;

public class MovieService : IMovieService
{
    private readonly List<Movie> _movies =
    [
        new Movie
        {
            Id = 1,
            Title = "Inception",
            ReleaseDate = 2010,
            Director = "Christopher Nolan",
            Genre = "Sci-Fi",
            Rating = 8.8
        },
        new Movie
        {
            Id = 2,
            Title = "Interstellar",
            ReleaseDate = 2014,
            Director = "Christopher Nolan",
            Genre = "Sci-Fi",
            Rating = 8.7
        },
        new Movie
        {
            Id = 3,
            Title = "The Dark Knight",
            ReleaseDate = 2008,
            Director = "Christopher Nolan",
            Genre = "Action",
            Rating = 9.0
        }
    ];

    private readonly List<Movie> _watchList = [];

    public IReadOnlyList<Movie> GetMovies() => _movies;

    public IReadOnlyList<Movie> GetWatchList() => _watchList;

    public Task AddToWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_watchList.Any(m => m.Id == movie.Id))
        {
            _watchList.Add(movie);
        }

        return Task.CompletedTask;
    }

    public Task RemoveFromWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _watchList.Remove(movie);
        return Task.CompletedTask;
    }

    public Movie? GetMovieById(int id)
    {
        return _movies.FirstOrDefault(m => m.Id == id);
    }
}
