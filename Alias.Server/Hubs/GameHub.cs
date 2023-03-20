using Microsoft.AspNetCore.SignalR;
using Alias.Server.Models;
using System.Security.Cryptography.X509Certificates;
using Alias.Server.Services;

namespace Alias.Server.Hubs;

public class GameHub : Hub<IGameHub>
{
    private readonly IGameService _gameService;
    private readonly IUserService _userService;

    public GameHub(IGameService gameService, IUserService userService)
    {
        _gameService = gameService;
        _userService = userService;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        User? user = _userService.RemoveUser(Context.ConnectionId);
        if(user is null) { return; }
        _gameService.RemoveUser(user);
        await base.OnDisconnectedAsync(exception);
    }

    public async void Connect(User user)
    {
        user.ConnectionId = Context.ConnectionId;
        if(await _gameService.AddUser(user))
        {
            _userService.AddUser(user);
        }
    }

    public void StartGame(GameStartOptions options)
    {
        User? user = _userService.GetUser(Context.ConnectionId);
        if (user is null) return;
        _gameService.StartGame(user, options);
    }

    public void Ping()
    {
        Clients.Caller.Ping();
    }

    public void GetConnectedPlayers()
    {
        User? user = _userService.GetUser(Context.ConnectionId);
        if (user is null) { return; }
        _gameService.GetConnectedUsers(user);
    }
} // class end
