using MovieWatchList.Components;
using MovieWatchList.Services;
using MovieWatchList.Services.Interfaces;
using MovieWatchList.ViewModels.Components;
using MovieWatchList.ViewModels.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<ITmdbService, TmdbService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ISeriesService, SeriesService>();
builder.Services.AddTransient<HomeViewModel>();
builder.Services.AddTransient<MovieDetailsViewModel>();
builder.Services.AddTransient<MovieListViewModel>();
builder.Services.AddTransient<MovieCardViewModel>();
builder.Services.AddTransient<SeriesDetailsViewModel>();
builder.Services.AddTransient<SeriesListViewModel>();
builder.Services.AddTransient<RealityListViewModel>();
builder.Services.AddTransient<SeriesCardViewModel>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();