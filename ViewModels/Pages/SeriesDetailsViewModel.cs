using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class SeriesDetailsViewModel : ViewModelBase
{
    private readonly ISeriesService _seriesService;
    private readonly ITmdbService _tmdbService;

    private Series? _series;
    public Series? Series
    {
        get => _series;
        private set => SetProperty(ref _series, value);
    }

    private bool _isInWatchList;
    public bool IsInWatchList
    {
        get => _isInWatchList;
        private set => SetProperty(ref _isInWatchList, value);
    }

    private string? _posterUrl;
    public string? PosterUrl
    {
        get => _posterUrl;
        private set => SetProperty(ref _posterUrl, value);
    }

    private Series? _previousSeries;
    public Series? PreviousSeries
    {
        get => _previousSeries;
        private set => SetProperty(ref _previousSeries, value);
    }

    private Series? _nextSeries;
    public Series? NextSeries
    {
        get => _nextSeries;
        private set => SetProperty(ref _nextSeries, value);
    }

    public SeriesDetailsViewModel(ISeriesService seriesService, ITmdbService tmdbService)
    {
        _seriesService = seriesService;
        _tmdbService = tmdbService;
    }

    public async Task LoadSeriesAsync(int id)
    {
        IsLoading = true;
        ErrorMessage = null;

        Series = null;
        PreviousSeries = null;
        NextSeries = null;
        PosterUrl = null;
        IsInWatchList = false;

        try
        {
            Series = await _seriesService.GetSeriesByIdAsync(id);

            if (Series is not null)
            {
                PosterUrl = await _tmdbService.GetTvPosterUrlAsync(Series.Title);
                IsInWatchList = _seriesService.GetWatchList().Any(s => s.Id == Series.Id);

                var allSeries = _seriesService.GetSeries();

                PreviousSeries = allSeries
                    .Where(s => s.Id < id)
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefault();

                NextSeries = allSeries
                    .Where(s => s.Id > id)
                    .OrderBy(s => s.Id)
                    .FirstOrDefault();
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong while loading the TV series.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task AddToWatchListAsync()
    {
        if (Series is null)
        {
            return;
        }

        await _seriesService.AddToWatchListAsync(Series);
        IsInWatchList = true;
    }
}
