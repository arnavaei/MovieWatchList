using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Components;

public class MovieCardViewModel : ViewModelBase
{
    private readonly ITmdbService _tmdbService;

    public string? PosterUrl { get; private set; }

    public bool IsLoadingPoster { get; private set; } = true;

    public MovieCardViewModel(ITmdbService tmdbService)
    {
        _tmdbService = tmdbService;
    }

    public async Task LoadPosterAsync(
        string movieTitle,
        CancellationToken cancellationToken = default)
    {
        PosterUrl = await _tmdbService.GetPosterUrlAsync(movieTitle, cancellationToken);
        IsLoadingPoster = false;
        OnPropertyChanged(nameof(PosterUrl));
        OnPropertyChanged(nameof(IsLoadingPoster));
    }
}
