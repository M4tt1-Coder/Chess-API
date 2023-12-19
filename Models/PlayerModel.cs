namespace Chess_API.Models;

public class PlayerModel
{
    public TimeSpan Time { get; set; }

    public List<FigureModel>? Pieces { get; set; }

    public string? Name { get; set; }

    public PlayerModel(TimeSpan time, string name)
    {
        Time = time;
        Pieces = new List<FigureModel>();
        Name = name;
    }
}