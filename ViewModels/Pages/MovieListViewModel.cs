using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class MovieListViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;

    private IReadOnlyList<Movie> _movies = [];
    public IReadOnlyList<Movie> Movies
    {
        get => _movies;
        private set => SetProperty(ref _movies, value);
    }

    private IReadOnlyList<Movie> _filteredMovies = [];
    public IReadOnlyList<Movie> FilteredMovies
    {
        get => _filteredMovies;
        private set => SetProperty(ref _filteredMovies, value);
    }

    private IReadOnlyList<Movie> _watchList = [];
    public IReadOnlyList<Movie> WatchList
    {
        get => _watchList;
        private set => SetProperty(ref _watchList, value);
    }

    private string _selectedGenre = "All";
    public string SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            if (SetProperty(ref _selectedGenre, value))
            {
                ApplyFilters();
            }
        }
    }

    private string _searchTerm = "";
    public string SearchTerm
    {
        get => _searchTerm;
        set
        {
            if (SetProperty(ref _searchTerm, value))
            {
                ApplyFilters();
            }
        }
    }

    public IReadOnlyList<string> AvailableGenres { get; private set; } = ["All"];

    public MovieListViewModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task AddToWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.AddToWatchListAsync(movie, cancellationToken);
        RefreshWatchList();
    }

    public async Task RemoveFromWatchListAsync(
        Movie movie,
        CancellationToken cancellationToken = default)
    {
        await _movieService.RemoveFromWatchListAsync(movie, cancellationToken);
        RefreshWatchList();
    }

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            Movies = _movieService.GetMovies();

            var genres = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "All" };
            foreach (var item in Movies)
            {
                var parts = item.Genre.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    genres.Add(part);
                }
            }
            AvailableGenres = genres.ToList();

            RefreshWatchList();
            ApplyFilters();
        }
        catch (Exception)
        {
            ErrorMessage = "Failed to load movies";
        }
        finally
        {
            IsLoading = false;
        }

        await Task.CompletedTask;
    }

    public Task LoacAsync() => LoadAsync();

    public void ApplyFilters()
    {
        var result = _movies.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim();
            result = result.Where(m =>
                m.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                m.Director.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                m.Genre.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(SelectedGenre, "All", StringComparison.OrdinalIgnoreCase))
        {
            result = result.Where(m => m.Genre.Contains(SelectedGenre, StringComparison.OrdinalIgnoreCase));
        }

        FilteredMovies = result.ToList();
    }

    private void RefreshWatchList()
    {
        WatchList = [.. _movieService.GetWatchList()];
    }
}
