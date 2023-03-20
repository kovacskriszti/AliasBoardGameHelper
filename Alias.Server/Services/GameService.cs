using Alias.Server.Hubs;
using Alias.Server.Models;
using Microsoft.AspNetCore.SignalR;
using System;

namespace Alias.Server.Services;

public class GameService : IGameService
{
    private readonly IHubContext<GameHub, IGameHub> _hubContext;
    private static List<Game> _games=new List<Game>();

    public GameService(IHubContext<GameHub, IGameHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<bool> AddUser(User user)
    {
        Game? game = FindGame(user.GameId);
        if (game is null)
        {
            game = CreateGame(user);
            return true;
        }
        return await game.AddUser(user);
    }

    public void RemoveUser(User user)
    {
        Game? game = FindGame(user.GameId);
        if(game is null) { return; }
        game.RemoveUser(user);
        RemoveGameIfEmpty(game);
    }

    public void StartGame(User user, GameStartOptions options)
    {
        Game? game = FindGame(user.GameId);
        if (game is null) { return; }
        game.StartGame(user, options);
    }

    public void GetConnectedUsers(User user)
    {
        Game? game = FindGame(user.GameId);
        if(game is null) { return; }
        game.GetConnectedPlayers(user);
}

    private Game? FindGame(string gameId)
    {
        return _games.FirstOrDefault(game => game.Id == gameId);
    }

    private Game CreateGame(User user)
    {
        Game game = new Game(_hubContext, user);
        lock (_games)
        {
            _games.Add(game);
        }
        return game;
    }

    private void RemoveGameIfEmpty(Game game)
    {
        if (game.EmptyUsers())
        {
            lock (_games)
            {
                _games.Remove(game);
            }
        }
    }
} // class end
