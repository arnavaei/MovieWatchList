using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class HomeViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;
    private readonly ISeriesService _seriesService;

    private IReadOnlyList<Movie> _featuredMovies = [];
    public IReadOnlyList<Movie> FeaturedMovies
    {
        get => _featuredMovies;
        private set => SetProperty(ref _featuredMovies, value);
    }

    private IReadOnlyList<Series> _featuredSeries = [];
    public IReadOnlyList<Series> FeaturedSeries
    {
        get => _featuredSeries;
        private set => SetProperty(ref _featuredSeries, value);
    }

    private IReadOnlyList<Movie> _movieWatchList = [];
    public IReadOnlyList<Movie> MovieWatchList
    {
        get => _movieWatchList;
        private set => SetProperty(ref _movieWatchList, value);
    }

    private IReadOnlyList<Series> _seriesWatchList = [];
    public IReadOnlyList<Series> SeriesWatchList
    {
        get => _seriesWatchList;
        private set => SetProperty(ref _seriesWatchList, value);
    }

    private int _totalMovies;
    public int TotalMovies
    {
        get => _totalMovies;
        private set => SetProperty(ref _totalMovies, value);
    }

    private int _totalSeries;
    public int TotalSeries
    {
        get => _totalSeries;
        private set => SetProperty(ref _totalSeries, value);
    }

    public int TotalWatchListCount => MovieWatchList.Count + SeriesWatchList.Count;

    public HomeViewModel(IMovieService movieService, ISeriesService seriesService)
    {
        _movieService = movieService;
        _seriesService = seriesService;
    }

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            var movies = _movieService.GetMovies();
            var series = _seriesService.GetSeries();

            TotalMovies = movies.Count;
            TotalSeries = series.Count;

            FeaturedMovies = movies.OrderByDescending(m => m.Rating).Take(4).ToList();
            FeaturedSeries = series.OrderByDescending(s => s.Rating).Take(4).ToList();

            RefreshWatchLists();
        }
        catch (Exception)
        {
            ErrorMessage = "Failed to load dashboard data.";
        }
        finally
        {
            IsLoading = false;
        }

        await Task.CompletedTask;
    }

    public async Task AddMovieToWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.AddToWatchListAsync(movie, cancellationToken);
        RefreshWatchLists();
    }

    public async Task RemoveMovieFromWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.RemoveFromWatchListAsync(movie, cancellationToken);
        RefreshWatchLists();
    }

    public async Task AddSeriesToWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        await _seriesService.AddToWatchListAsync(series, cancellationToken);
        RefreshWatchLists();
    }

    public async Task RemoveSeriesFromWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        await _seriesService.RemoveFromWatchListAsync(series, cancellationToken);
        RefreshWatchLists();
    }

    private void RefreshWatchLists()
    {
        MovieWatchList = [.. _movieService.GetWatchList()];
        SeriesWatchList = [.. _seriesService.GetWatchList()];
        OnPropertyChanged(nameof(TotalWatchListCount));
    }
}
