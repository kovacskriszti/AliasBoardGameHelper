using Alias.Server.Hubs;

namespace Alias.Server.Models;

public class Team
{
    public string Id { get; set; } = null!;
    public List<User> Users { get; set; } = new List<User>();
    Dictionary<int, Queue<string>> Words = new Dictionary<int, Queue<string>>();
    public short Position { get; set; }
}
