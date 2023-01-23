using Alias.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.X509Certificates;

namespace Alias.Server.Models;

public class Game
{
    public string Id { get; set; } = null!;
    public List<User> Users { get; set; } = new List<User>();
    public Dictionary<int, List<string>> Words { get; set; } = new Dictionary<int, List<string>>();
    public Team[] Teams { get; set; } = null!;
	private readonly IHubContext<GameHub, IGameHub> _hubContext;

	public Game(User user, IHubContext<GameHub, IGameHub> hubContext)
    {
		_hubContext = hubContext;
		Id = user.GameId;
        user.Admin = true;
        AddUser(user);
    }

    public async Task<bool> AddUser(User user)
    {
        if(UserExists(user))
        {
			_hubContext.Clients.Client(user.ConnectionId).UserTaken();
            return false;
		}
        lock(Users)
        { 
        Users.Add(user);
        }
		_hubContext.Groups.AddToGroupAsync(user.ConnectionId, user.GameId);
        _hubContext.Clients.Client(user.ConnectionId).SuccessConnection(user);
        _hubContext.Clients.Groups(Id).Connect(user.Name);
        return true;
	}

	public void RemoveUser(User user)
    {
        Users.Remove(user);
		_hubContext.Clients.Group(Id).Disconnect(user.Name);
        if(user.Admin && !EmptyUsers())
        {
            PassAdmin();
        }
	}

    public void Start(string connectionId, GameStartOptions gameStartOptions)
    {
        if(!CheckIfIsAdmin(connectionId))
        {
            return;
        }
    }

	public void SendConnectedPlayers(string connectionId)
	{
		if (CheckIfIsAdmin(connectionId))
		{
			string[] users = Users.Select(usr => usr.Name).ToArray();
			_hubContext.Clients.Client(connectionId).SendConnectedPlayers(users);
		}
	}

public bool EmptyUsers()
    {
        if (Users.Count() < 1)
            return true;
        return false;
    }

    public User? GetAdmin()
    {
        User? admin = Users.FirstOrDefault(usr => usr.Admin = true);
        if(admin is null)
        {
            return PassAdmin();
        }
        return admin;
    }

	private bool UserExists(User user)
	{
		if (Users.FirstOrDefault(usr => usr.ConnectionId == user.ConnectionId || usr.Name == user.Name) is not null)
		{
			return true;
		}
		return false;
	}

	private User? PassAdmin()
	{
		User newAdmin = Users[0];
		lock (Users)
		{
			newAdmin.Admin = true;
		}
		_hubContext.Clients.Client(newAdmin.ConnectionId).TransferAdmin();
		return newAdmin;
	}

	private bool CheckIfIsAdmin(string connectionId)
	{
		if (Users.FirstOrDefault(usr => usr.ConnectionId==connectionId&&usr.Admin==true) is not null)
            {
            return true;
        }
        return false;
	}
} // class end
