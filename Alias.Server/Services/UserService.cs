using Alias.Server.Models;

namespace Alias.Server.Services;

public class UserService : IUserService
{
    private static HashSet<User> _users = new HashSet<User>();

    public UserService()
    {

    }

    public void AddUser(User user)
    {
        lock (_users)
        {
            _users.Add(user);
        }
    }

    public User? RemoveUser(string connectionId)
    {
        User? user = GetUser(connectionId);
        if(user is null) { return null; }
        lock(_users)
        {
            _users.Remove(user);
            return user;
        }
    }

    public User? GetUser(string connectionId)
    {
        return _users.FirstOrDefault(usr => usr.ConnectionId == connectionId);
    }
}
