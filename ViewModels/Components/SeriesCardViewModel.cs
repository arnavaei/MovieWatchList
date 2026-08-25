using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Components;

public class SeriesCardViewModel : ViewModelBase, IDisposable
{
    private readonly ITmdbService _tmdbService;
    private readonly CancellationTokenSource _cts = new();

    private string? _posterUrl;
    public string? PosterUrl
    {
        get => _posterUrl;
        private set => SetProperty(ref _posterUrl, value);
    }

    private bool _isLoadingPoster = true;
    public bool IsLoadingPoster
    {
        get => _isLoadingPoster;
        private set => SetProperty(ref _isLoadingPoster, value);
    }

    public SeriesCardViewModel(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task LoadPosterAsync(
        string seriesTitle,
        CancellationToken cancellationToken = default)
    {
        using var linkedCts = CancellationTokenSource
            .CreateLinkedTokenSource(cancellationToken, _cts.Token);
        IsLoadingPoster = true;
        ErrorMessage = null;

        try
        {
            PosterUrl = await _tmdbService.GetTvPosterUrlAsync(
                seriesTitle,
                linkedCts.Token);
        }
        catch (OperationCanceledException)
        {
            // Operation Cancelled
        }
        catch (Exception)
        {
            ErrorMessage = "Failed to load TV series poster.";
        }
        finally
        {
            IsLoadingPoster = false;
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
