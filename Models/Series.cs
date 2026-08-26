namespace MovieWatchList.Models;

public class Series
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Genre { get; set; } = "";
    public string Creator { get; set; } = "";
    public int StartYear { get; set; }
    public int? EndYear { get; set; }
    public int Seasons { get; set; }
    public int Episodes { get; set; }
    public string Status { get; set; } = "Ongoing"; // "Ongoing", "Ended", "Miniseries"
    public string Network { get; set; } = "";
    public double Rating { get; set; }
    public string Overview { get; set; } = "";
    public bool IsRealityShow { get; set; }
}
