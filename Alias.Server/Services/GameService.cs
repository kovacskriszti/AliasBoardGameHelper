using Alias.Server.Hubs;
using Alias.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace Alias.Server.Services;

public class GameService : IGameService
{
	private static List<Game> Games = new List<Game>();
	private readonly IHubContext<GameHub, IGameHub> _hubContext;

	public GameService(IHubContext<GameHub, IGameHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public async Task<bool> Connect(User user)
	{
		Game? game = FindGame(user.GameId);
		if (game is null)
		{
			game = CreateGame(user);
			return true;
		}
		return await game.AddUser(user);
	}

	public void DisconnectUser(User user)
	{
		Game? game = FindGame(user.GameId);
		if (game is null || user.ConnectionId == null)
		{
			return;
		}
		game.RemoveUser(user);
		RemoveGameIfEmpty(game);
	}

public void GetConnectedPlayers(User user)
	{
		Game? game = FindGame(user.GameId);
		if(game is null)
		{
			return;
		}
		game.SendConnectedPlayers(user.ConnectionId);
	}

	public void StartGame(string connectionId, GameStartOptions gameStartOptions)
	{
		if(gameStartOptions.Id==null||gameStartOptions.Random==null)
		{
			return;
		}
		Game? game = FindGame(gameStartOptions.Id);
		if(game is null)
		{
			return;
		}
		game.Start(connectionId, gameStartOptions);
	}

	private Game? FindGame(string gameId)
	{
		return Games.FirstOrDefault(game => game.Id == gameId);
	}

private Game CreateGame(User user)
{
	Game game = new Game(user, _hubContext);
	lock (Games)
	{
		Games.Add(game);
	}
	return game;
}

	private bool RemoveGameIfEmpty(Game game)
	{
		if (game.EmptyUsers())
		{
			lock (Games)
			{
				Games.Remove(game);
				return true;
			}
		}
		return false;
	}
} // class end
