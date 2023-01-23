namespace Alias.Server.Models;

public class User
{
	public string? ConnectionId { get; set; }
	public string GameId { get; set; } = null!;
	public string Name { get; set; } = null!;
	public bool Admin { get; set; } = false;
}