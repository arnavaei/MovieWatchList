using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Base;

namespace MovieWatchList.ViewModels.Pages;

public class MovieDetailsViewModel : ViewModelBase
{
    private readonly IMovieService _movieService;

    public Movie? Movie { get; private set; }

    public Movie? PreviousMovie { get; private set; }

    public Movie? NextMovie { get; private set; }

    public MovieDetailsViewModel(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task LoadMovieAsync(int id)
    {
        IsLoading = true;
        ErrorMessage = null;

        Movie = null;
        PreviousMovie = null;
        NextMovie = null;

        try
        {
            Movie = await _movieService.GetMovieByIdAsync(id);

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
}