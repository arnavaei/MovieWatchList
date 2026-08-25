using MovieWatchList.Models;
using MovieWatchList.Services.Interfaces;

namespace MovieWatchList.Services;

public class SeriesService : ISeriesService
{
    private readonly List<Series> _series =
    [
        new Series
        {
            Id = 1,
            Title = "Breaking Bad",
            Genre = "Crime / Drama",
            Creator = "Vince Gilligan",
            StartYear = 2008,
            EndYear = 2013,
            Seasons = 5,
            Episodes = 62,
            Status = "Ended",
            Network = "AMC",
            Rating = 9.5,
            Overview = "A chemistry teacher diagnosed with inoperable lung cancer turns to manufacturing and selling methamphetamine with a former student in order to secure his family's financial future."
        },
        new Series
        {
            Id = 2,
            Title = "Succession",
            Genre = "Drama",
            Creator = "Jesse Armstrong",
            StartYear = 2018,
            EndYear = 2023,
            Seasons = 4,
            Episodes = 39,
            Status = "Ended",
            Network = "HBO",
            Rating = 8.9,
            Overview = "The Roy family is known for controlling the biggest media and entertainment company in the world. However, their world will change when their aging father steps down from the company."
        },
        new Series
        {
            Id = 3,
            Title = "Severance",
            Genre = "Sci-Fi / Thriller",
            Creator = "Dan Erickson",
            StartYear = 2022,
            EndYear = null,
            Seasons = 2,
            Episodes = 19,
            Status = "Ongoing",
            Network = "Apple TV+",
            Rating = 8.7,
            Overview = "Mark leads a team of office workers whose memories have been surgically divided between their work and personal lives. When a mysterious colleague appears outside of work, it begins a journey to discover the truth about their jobs."
        },
        new Series
        {
            Id = 4,
            Title = "Chernobyl",
            Genre = "Drama / History",
            Creator = "Craig Mazin",
            StartYear = 2019,
            EndYear = 2019,
            Seasons = 1,
            Episodes = 5,
            Status = "Miniseries",
            Network = "HBO",
            Rating = 9.4,
            Overview = "In April 1986, an explosion at the Chernobyl nuclear power plant in the Union of Soviet Socialist Republics becomes one of the world's worst man-made catastrophes."
        },
        new Series
        {
            Id = 5,
            Title = "The Sopranos",
            Genre = "Crime / Drama",
            Creator = "David Chase",
            StartYear = 1999,
            EndYear = 2007,
            Seasons = 6,
            Episodes = 86,
            Status = "Ended",
            Network = "HBO",
            Rating = 9.2,
            Overview = "New Jersey mob boss Tony Soprano deals with personal and professional issues in his home and business life that's affecting his mental state, leading him to seek professional psychiatric counseling."
        },
        new Series
        {
            Id = 6,
            Title = "Better Call Saul",
            Genre = "Crime / Drama",
            Creator = "Vince Gilligan, Peter Gould",
            StartYear = 2015,
            EndYear = 2022,
            Seasons = 6,
            Episodes = 63,
            Status = "Ended",
            Network = "AMC",
            Rating = 9.0,
            Overview = "The trials and tribulations of criminal lawyer Jimmy McGill in the years leading up to his fateful run-in with Walter White and Jesse Pinkman."
        },
        new Series
        {
            Id = 7,
            Title = "The Wire",
            Genre = "Crime / Drama",
            Creator = "David Simon",
            StartYear = 2002,
            EndYear = 2008,
            Seasons = 5,
            Episodes = 60,
            Status = "Ended",
            Network = "HBO",
            Rating = 9.3,
            Overview = "The Baltimore drug scene, as seen through the eyes of drug dealers and law enforcement."
        },
        new Series
        {
            Id = 8,
            Title = "The Last of Us",
            Genre = "Drama / Sci-Fi",
            Creator = "Craig Mazin, Neil Druckmann",
            StartYear = 2023,
            EndYear = null,
            Seasons = 2,
            Episodes = 16,
            Status = "Ongoing",
            Network = "HBO",
            Rating = 8.8,
            Overview = "After a global pandemic destroys civilization, a hardened survivor takes charge of a 14-year-old girl who may be humanity's last hope."
        },
        new Series
        {
            Id = 9,
            Title = "Stranger Things",
            Genre = "Sci-Fi / Horror",
            Creator = "The Duffer Brothers",
            StartYear = 2016,
            EndYear = 2025,
            Seasons = 5,
            Episodes = 42,
            Status = "Ended",
            Network = "Netflix",
            Rating = 8.7,
            Overview = "When a young boy vanishes, a small town uncovers a mystery involving secret experiments, terrifying supernatural forces and one strange little girl."
        },
        new Series
        {
            Id = 10,
            Title = "True Detective",
            Genre = "Crime / Mystery",
            Creator = "Nic Pizzolatto",
            StartYear = 2014,
            EndYear = null,
            Seasons = 4,
            Episodes = 30,
            Status = "Ongoing",
            Network = "HBO",
            Rating = 8.9,
            Overview = "Anthology series in which police investigations unearth the personal and professional secrets of those involved, both within and outside the law."
        },
        new Series
        {
            Id = 11,
            Title = "Dark",
            Genre = "Sci-Fi / Mystery",
            Creator = "Baran bo Odar, Jantje Friese",
            StartYear = 2017,
            EndYear = 2020,
            Seasons = 3,
            Episodes = 26,
            Status = "Ended",
            Network = "Netflix",
            Rating = 8.7,
            Overview = "A family saga with a supernatural twist, set in a German town where the disappearance of two young children exposes the relationships among four families."
        },
        new Series
        {
            Id = 12,
            Title = "Peaky Blinders",
            Genre = "Crime / Drama",
            Creator = "Steven Knight",
            StartYear = 2013,
            EndYear = 2022,
            Seasons = 6,
            Episodes = 36,
            Status = "Ended",
            Network = "BBC",
            Rating = 8.8,
            Overview = "A gangster family epic set in 1900s England, centering on a gang who sew razor blades in the peaks of their caps, and their fierce boss Tommy Shelby."
        }
    ];

    private readonly List<Series> _watchList = [];

    public IReadOnlyList<Series> GetSeries() => _series;

    public IReadOnlyList<Series> GetWatchList() => _watchList;

    public Task AddToWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_watchList.Any(s => s.Id == series.Id))
        {
            _watchList.Add(series);
        }

        return Task.CompletedTask;
    }

    public Task RemoveFromWatchListAsync(
        Series series,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _watchList.Remove(series);
        return Task.CompletedTask;
    }

    public async Task<Series?> GetSeriesByIdAsync(int id)
    {
        return await Task.FromResult(
            _series.FirstOrDefault(s => s.Id == id));
    }
}
