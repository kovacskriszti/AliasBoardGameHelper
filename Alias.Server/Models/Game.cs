using Alias.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.X509Certificates;

namespace Alias.Server.Models;

public class Game
{
    public string Id { get; set; } = null!;
    public List<User> Users { get; set; } = new List<User>();
    public Team[] Teams { get; set; } = null!;
    private bool Started = false;
    private readonly IHubContext<GameHub, IGameHub> _hubContext;

    public Game(IHubContext<GameHub, IGameHub> hubContext, User user)
    {
        _hubContext = hubContext;
        Id = user.GameId;
        user.Admin = true;
        AddUser(user);
    }

    public async Task<bool> AddUser(User user)
    {
        if (UserExists(user))
        {
            _hubContext.Clients.Client(user.ConnectionId).UserTaken();
            return false;
        }
        lock (Users)
        {
            Users.Add(user);
        }
        if (user.Admin) _hubContext.Clients.Client(user.ConnectionId).SuccessConnectionAdmin();
        else _hubContext.Clients.Client(user.ConnectionId).SuccessConnection();
        await _hubContext.Groups.AddToGroupAsync(user.ConnectionId, user.GameId);
        _hubContext.Clients.Group(Id).Connect(user.Name);
        return true;
    }

    public void RemoveUser(User user)
    {
        Users.Remove(user);
        _hubContext.Clients.Group(Id).Disconnect(user.Name);
        if (user.Admin && !EmptyUsers())
        {
            PassAdmin();
        }
    }

    public void StartGame(User user, GameStartOptions startOptions)
    {
        if (!CheckIfIsAdmin(user.ConnectionId)) { return; }
        if(startOptions.Random)
        {
            GenerateRandomTeams(startOptions.NumberOfTeams);
        }
        else
        {
            GenerateTeams(startOptions);
        }
    }

    public void GetConnectedPlayers(User user)
    {
        if (!CheckIfIsAdmin(user.ConnectionId)) { return; }
            string[] users = Users.Select(usr => usr.Name).ToArray();
        _hubContext.Clients.Client(user.ConnectionId).SendConnectedPlayers(users);
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
        if (admin is null)
        {
            return PassAdmin();
        }
        return admin;
    }

    public bool UserExists(User user)
    {
        if (Users.FirstOrDefault(usr => usr.ConnectionId == user.ConnectionId || usr.Name == user.Name) is not null)
        {
            return true;
        }
        return false;
    }

    public bool UserExists(string name)
    {
        if (Users.FirstOrDefault(usr => usr.Name == name) is not null)
        {
            return true;
        }
        return false;
    }

    private User? PassAdmin()
    {
        if(Users.Count()<1) { return null; }
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
        if (Users.FirstOrDefault(usr => usr.ConnectionId == connectionId && usr.Admin == true) is not null)
        {
            return true;
        }
        return false;
    }

    private bool GenerateRandomTeams(int numberOfTeams)
    {
        Random random = new Random();
        int numberOfPlayers = Users.Count();
        if(numberOfPlayers<4||numberOfPlayers<numberOfTeams*2) { return false;  }
        Team[] teams = new Team[numberOfTeams];
        List<User> userAllocator = Users;
        while(userAllocator.Count()>0)
        {
            for(int i=0; i<teams.Length; i++)
            {
                int randomUserKey = random.Next(0, userAllocator.Count());
                if (teams[i] is default(Team))
                {
                    teams[i] = new Team();
                }
                teams[i].AddUser(userAllocator[randomUserKey]);
                userAllocator.RemoveAt(randomUserKey);
            }
        }
        Teams = teams;
        return true;
    }

    private void GenerateRound()
    {
        throw new NotImplementedException();
    }

    private User? GetUserByName(string name)
    {
        return Users.FirstOrDefault(usr => usr.Name == name);
    }

    private void GenerateTeams(GameStartOptions startOptions)
    {
        if(!ValidateTeams(startOptions)) { return; }
        Team[] teams = new Team[startOptions.NumberOfTeams];
        for(int i=0; i<startOptions.NumberOfTeams; i++)
        {
            teams[i] = new Team();
        }
        foreach(UserTeam user in startOptions.Teams)
        {
            teams[user.Team-1].AddUser(GetUserByName(user.User));
        }
        Teams = teams;
    }

    private bool ValidateTeams(GameStartOptions startOptions)
    {
        if(startOptions.Teams is null||startOptions.Teams.Count()<startOptions.NumberOfTeams) { return false; }
        int[] membersInTeam = new int[startOptions.NumberOfTeams + 1];
        foreach (UserTeam user in startOptions.Teams)
        {
            if (!UserExists(user.User))
            {
                return false;
            }
            membersInTeam[user.Team]++;
        }
        for (int i = 1; i < membersInTeam.Length; i++)
        {
            if (membersInTeam[i] < 2)
            {
                return false;
            }
        }
        return true;
    }
} // class end
