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
        },
        new Series
        {
            Id = 13,
            Title = "Survivor",
            Genre = "Competition / Adventure",
            Creator = "Charlie Parsons",
            StartYear = 2000,
            EndYear = null,
            Seasons = 48,
            Episodes = 700,
            Status = "Ongoing",
            Network = "CBS",
            Rating = 7.5,
            Overview = "A group of contestants are stranded in a remote location with little more than the clothes on their backs. The tribes compete in challenges for rewards and immunity, while alliances and strategy decide who is voted out until one Sole Survivor remains.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 14,
            Title = "The Amazing Race",
            Genre = "Competition / Travel",
            Creator = "Elise Doganieri, Bertram van Munster",
            StartYear = 2001,
            EndYear = null,
            Seasons = 37,
            Episodes = 430,
            Status = "Ongoing",
            Network = "CBS",
            Rating = 7.6,
            Overview = "Teams of two race around the world, completing challenges and navigating foreign cities in a competition of speed, endurance, and problem-solving.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 15,
            Title = "The Great British Bake Off",
            Genre = "Competition / Food",
            Creator = "Anna Beattie",
            StartYear = 2010,
            EndYear = null,
            Seasons = 15,
            Episodes = 140,
            Status = "Ongoing",
            Network = "Channel 4",
            Rating = 8.6,
            Overview = "Amateur bakers compete in a series of challenges, attempting to impress a panel of judges with their baking skills in a quaint English countryside tent.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 16,
            Title = "Clarkson's Farm",
            Genre = "Reality / Comedy",
            Creator = "Jeremy Clarkson",
            StartYear = 2021,
            EndYear = null,
            Seasons = 5,
            Episodes = 40,
            Status = "Ongoing",
            Network = "Prime Video",
            Rating = 9,
            Overview = "Follow Jeremy Clarkson as he embarks on his latest adventure, farming. The man who on several occasions claims to be allergic to manual labour takes on the most manually labour intensive job there is. What could possibly go wrong?",
            IsRealityShow = true
        },
        new Series
        {
            Id = 17,
            Title = "The Traitors",
            Genre = "Competition / Mystery",
            Creator = "Marc Posch",
            StartYear = 2023,
            EndYear = null,
            Seasons = 3,
            Episodes = 33,
            Status = "Ongoing",
            Network = "Peacock",
            Rating = 8.0,
            Overview = "A group of contestants live together in a castle, some secretly assigned as Traitors. Faithfuls must uncover the traitors before they are murdered, while traitors try to survive until the end.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 18,
            Title = "Queer Eye",
            Genre = "Makeover / Lifestyle",
            Creator = "David Collins",
            StartYear = 2018,
            EndYear = null,
            Seasons = 8,
            Episodes = 66,
            Status = "Ongoing",
            Network = "Netflix",
            Rating = 8.5,
            Overview = "The Fab Five help people improve their lives with advice on fashion, food, grooming, culture, and home design — one makeover at a time.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 19,
            Title = "Top Chef",
            Genre = "Competition / Food",
            Creator = "Magical Elves",
            StartYear = 2006,
            EndYear = null,
            Seasons = 21,
            Episodes = 300,
            Status = "Ongoing",
            Network = "Bravo",
            Rating = 7.6,
            Overview = "Chefs compete in culinary challenges judged by a panel of professional chefs and food critics, with the winner earning the title of Top Chef.",
            IsRealityShow = true
        },
        new Series
        {
            Id = 20,
            Title = "The Voice",
            Genre = "Competition / Music",
            Creator = "John de Mol",
            StartYear = 2011,
            EndYear = null,
            Seasons = 26,
            Episodes = 550,
            Status = "Ongoing",
            Network = "NBC",
            Rating = 6.5,
            Overview = "Aspiring singers audition blindly for celebrity coaches, then compete through battles, knockouts, and live shows for a recording contract and the title of The Voice.",
            IsRealityShow = true
        }
    ];

    private readonly List<Series> _watchList = [];

    public IReadOnlyList<Series> GetSeries() =>
        _series.Where(s => !s.IsRealityShow).ToList();

    public IReadOnlyList<Series> GetRealityShows() =>
        _series.Where(s => s.IsRealityShow).ToList();

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
