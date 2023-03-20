using Alias.Server.Models;

namespace Alias.Server.Services;

public interface IUserService
{
    void AddUser(User user);
    User? GetUser(string connectionId);
    User? RemoveUser(string connectionId);
}
