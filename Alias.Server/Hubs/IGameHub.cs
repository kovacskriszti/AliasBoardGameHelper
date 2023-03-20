using Alias.Server.Models;

namespace Alias.Server.Hubs;

public interface IGameHub
{
    Task SuccessConnection();
    Task SuccessConnectionAdmin();
    Task UserTaken();
    Task Ping();
    Task Connect(string name);
    Task Disconnect(string name);
    Task TransferAdmin();
    Task SendConnectedPlayers(string[] playersName);

}
