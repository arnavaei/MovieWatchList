using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class MovieListViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;

    public IReadOnlyList<Movie> Movies { get; private set; } = [];

    private IReadOnlyList<Movie> _WatchList = [];

    public IReadOnlyList<Movie> WatchList
    {
        get => _WatchList;
        private set => SetProperty(ref _WatchList, value);
    }

    public MovieListViewModel(IMovieService movieService)
    {
        _movieService = movieService;
        Movies = _movieService.GetMovies();
        WatchList = _movieService.GetWatchList();
    }

    public async Task AddToWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.AddToWatchListAsync(movie, cancellationToken);
        WatchList = _movieService.GetWatchList();
    }

    public async Task RemoveFromWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.RemoveFromWatchListAsync(movie, cancellationToken);
        WatchList = _movieService.GetWatchList();
    }
}
