using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class MovieDetailsViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;
    private readonly ITmdbService _tmdbService;

    public Movie? Movie { get; private set; }
    
    public bool IsInWatchList {get; private set;}

    public string? PosterUrl { get; private set; }

    public Movie? PreviousMovie { get; private set; }

    public Movie? NextMovie { get; private set; }

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
            await _movieService.AddToWatchListAsync(Movie);
            IsInWatchList = true;
        }
    }
}