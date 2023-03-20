using Alias.Server.Models;

namespace Alias.Server.Services;

public interface IGameService
{
    Task<bool> AddUser(User user);
    void RemoveUser(User user);
    void StartGame(User user, GameStartOptions options);
    void GetConnectedUsers(User user);
}
