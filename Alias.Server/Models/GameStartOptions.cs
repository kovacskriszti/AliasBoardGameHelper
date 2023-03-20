namespace Alias.Server.Models;

public class GameStartOptions
{
    public string GameId { get; set; } = null!;
    public bool Random { get; set; }
    public short NumberOfTeams { get; set; }
    public List<UserTeam>? Teams { get; set; } = null;
}

public class UserTeam
{
    public string User { get; set; } = null!;
    public int Team { get; set; }
}