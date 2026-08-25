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
        },
        
        new Movie
        {
        Id = 4,
        Title = "Oppenheimer",
        ReleaseDate = 2023,
        Director = "Christopher Nolan",
        Genre = "Thriller",
        Rating = 8.2
        },
        
        new Movie
        {
        Id = 5,
        Title = "A Clockwork Orange",
        ReleaseDate = 1971,
        Director = "Stanley Kubrick",
        Genre = "Crime",
        Rating = 8.6
        },
        
        new Movie
        {
            Id = 6,
            Title = "Reservoir Dogs",
            ReleaseDate = 1992,
            Director = "Quentin Tarantino",
            Genre = "Thriller",
            Rating = 8.1
        },
        
        new Movie
        {
            Id = 7,
            Title = "Parasite",
            ReleaseDate = 2019,
            Director = "Bong Joon Ho",
            Genre = "Thriller",
            Rating = 8.5
        },
        
        new Movie
        {
            Id = 8,
            Title = "Fight Club",
            ReleaseDate = 1999,
            Director = "David Fincher",
            Genre = "Action",
            Rating = 8.8
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

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        return await Task.FromResult(
            _movies.FirstOrDefault(movie => movie.Id == id));
    }
}
