using Alias.Server.Models;

namespace Alias.Server.Services;

public interface IGameService
{
	void StartGame(string connectionId, GameStartOptions options);
	Task<bool> Connect(User user);
	void DisconnectUser(User user);
	void GetConnectedPlayers(User user);
}
