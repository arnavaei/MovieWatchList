using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Components;

public class MovieCardViewModel : ViewModelBase, IDisposable
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

    public MovieCardViewModel(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task LoadPosterAsync(
        string movieTitle,
        CancellationToken cancellationToken = default)
    {
        using var linkedCts = CancellationTokenSource
            .CreateLinkedTokenSource(cancellationToken, _cts.Token);
        IsLoadingPoster = true;
        ErrorMessage = null;

        try
        {
            PosterUrl = await _tmdbService.GetPosterUrlAsync(
                movieTitle,
                linkedCts.Token);
        }
        catch (OperationCanceledException)
        {
            // Operation Cancelled
        }
        catch (Exception)
        {
            ErrorMessage = "Failed to load movie poster.";
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