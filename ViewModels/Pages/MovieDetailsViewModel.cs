using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class MovieDetailsViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;
    private readonly ITmdbService _tmdbService;

    private Movie? _movie;

    public Movie? Movie
    {
        get => _movie;
        private set => SetProperty(ref _movie, value);
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

    private Movie? _previousMovie;

    public Movie? PreviousMovie
    {
        get => _previousMovie;
        private set => SetProperty(ref _previousMovie, value);
    }

    private Movie? _nextMovie;

    public Movie? NextMovie
    {
        get => _nextMovie;
        private set => SetProperty(ref _nextMovie, value);
    }

    public MovieDetailsViewModel(IMovieService movieService, ITmdbService tmdbService)
    {
        _movieService = movieService;
        _tmdbService = tmdbService;
    }

    public async Task LoadMovieAsync(int id)
    {
        IsLoading = true;
        ErrorMessage = null;

        Movie = null;
        PreviousMovie = null;
        NextMovie = null;
        PosterUrl = null;
        IsInWatchList = false;
        
        try
        {
            Movie = await _movieService.GetMovieByIdAsync(id);

            if (Movie is not null)
            {
                PosterUrl = await _tmdbService.GetPosterUrlAsync(Movie.Title);
                
                IsInWatchList = _movieService.GetWatchList().Any(movie => movie.Id == Movie.Id);
            }
            
            if (Movie is null)
            {
                return;
            }

            var movies = _movieService.GetMovies();

            PreviousMovie = movies
                .Where(movie => movie.Id < id)
                .OrderByDescending(movie => movie.Id)
                .FirstOrDefault();

            NextMovie = movies
                .Where(movie => movie.Id > id)
                .OrderBy(movie => movie.Id)
                .FirstOrDefault();
        }
        catch (Exception)
        {
            ErrorMessage = "Something went wrong while loading the movie.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task AddToWatchListAsync()
    {
        if (Movie is null)
        {
            return;
        }

        await _movieService.AddToWatchListAsync(Movie);
        IsInWatchList = true;
    }
}