namespace Alias.Server.Models;

public class GameStartOptions
{
	public string? Id { get; set; }
	public bool? Random { get; set; }
	public UserTeam[]? Teams { get; set; }
}

public class UserTeam
{
	public string? User { get; set; }
	public int Team { get; set; }
}