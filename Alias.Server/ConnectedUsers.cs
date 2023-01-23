#nullable disable
namespace Alias.Server;

public class ConnectedUsers
{
	private Dictionary<string, string> _users = new Dictionary<string, string>();

	public void Add(string user, string connectionId)
	{
		if(user.Length<1||connectionId.Length<1 || _users.TryGetValue(user, out connectionId))
		{
			return;
		}
		lock(_users)
		{
			_users.Add(user, connectionId);
		}
	}
}
