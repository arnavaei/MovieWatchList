using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Components;

public class MovieCardViewModel : ViewModelBase
{
    private readonly ITmdbService _tmdbService;

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
        IsLoadingPoster = true;
        ErrorMessage = null;

        try
        {
            PosterUrl = await _tmdbService.GetPosterUrlAsync(
                movieTitle,
                cancellationToken);
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
}