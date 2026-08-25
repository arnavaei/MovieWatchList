using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class SeriesListViewModel : ViewModelBase
{
    private readonly ISeriesService _seriesService;

    private IReadOnlyList<Series> _seriesList = [];
    public IReadOnlyList<Series> SeriesList
    {
        get => _seriesList;
        private set => SetProperty(ref _seriesList, value);
    }

    private IReadOnlyList<Series> _filteredSeries = [];
    public IReadOnlyList<Series> FilteredSeries
    {
        get => _filteredSeries;
        private set => SetProperty(ref _filteredSeries, value);
    }

    private IReadOnlyList<Series> _watchList = [];
    public IReadOnlyList<Series> WatchList
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

    private string _selectedStatus = "All";
    public string SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            if (SetProperty(ref _selectedStatus, value))
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
    public IReadOnlyList<string> AvailableStatuses { get; } = ["All", "Ongoing", "Ended", "Miniseries"];

    public int TotalSeasons => _seriesList.Sum(s => s.Seasons);
    public int TotalEpisodes => _seriesList.Sum(s => s.Episodes);

    public SeriesListViewModel(ISeriesService seriesService)
    {
        _seriesService = seriesService;
    }

    public async Task LoadAsync()
    {
        IsLoading = true;
        ErrorMessage = null;

        try
        {
            SeriesList = _seriesService.GetSeries();
            
            // Extract distinct genres
            var genres = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "All" };
            foreach (var item in SeriesList)
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
            ErrorMessage = "Failed to load TV series archive.";
        }
        finally
        {
            IsLoading = false;
        }

        await Task.CompletedTask;
    }

    public void ApplyFilters()
    {
        var result = _seriesList.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            var term = SearchTerm.Trim();
            result = result.Where(s =>
                s.Title.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.Creator.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.Network.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                s.Genre.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(SelectedGenre, "All", StringComparison.OrdinalIgnoreCase))
        {
            result = result.Where(s => s.Genre.Contains(SelectedGenre, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.Equals(SelectedStatus, "All", StringComparison.OrdinalIgnoreCase))
        {
            result = result.Where(s => string.Equals(s.Status, SelectedStatus, StringComparison.OrdinalIgnoreCase));
        }

        FilteredSeries = result.ToList();
    }

    public async Task AddToWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        await _seriesService.AddToWatchListAsync(series, cancellationToken);
        RefreshWatchList();
    }

    public async Task RemoveFromWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        await _seriesService.RemoveFromWatchListAsync(series, cancellationToken);
        RefreshWatchList();
    }

    private void RefreshWatchList()
    {
        WatchList = [.. _seriesService.GetWatchList()];
    }
}
