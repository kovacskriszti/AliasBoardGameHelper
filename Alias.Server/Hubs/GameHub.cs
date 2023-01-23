using Microsoft.AspNetCore.SignalR;
using Alias.Server.Models;
using Alias.Server.Services;

namespace Alias.Server.Hubs;

public class GameHub : Hub<IGameHub>
{
	private readonly IGameService _gameService;
	private static HashSet<User> _users { get; set; } = new HashSet<User>();

	public GameHub(IGameService gameService)
	{
		_gameService = gameService;
	}

	public override async Task OnConnectedAsync()
	{
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		User? user = GetUser(Context.ConnectionId);
		if(user is null)
		{
			return;
		}
		if(exception is null)
		{
		_gameService.DisconnectUser(user);
			lock(_users) 
			{ 
			_users.Remove(user);
			}
		}
		await base.OnDisconnectedAsync(exception);
	}

	public async Task Connect(User user)
	{
		user.ConnectionId=Context.ConnectionId;
		if(await _gameService.Connect(user))
		{
		lock(_users)
		{
			_users.Add(user);
		}
		}
	}

	public void Ping()
	{
		Clients.Caller.Ping();
	}

	public void GetConnectedPlayers(User user)
	{
		user.ConnectionId = Context.ConnectionId;
		_gameService.GetConnectedPlayers(user);
	}

	public void StartGame(GameStartOptions options)
	{
		_gameService.StartGame(Context.ConnectionId, options);
	}

	private User? GetUser(string connectionId)
	{
		return _users.FirstOrDefault(usr => usr.ConnectionId == Context.ConnectionId);
	}
} // class end
