using Alias.Server.Hubs;
using System.Text;

namespace Alias.Server.Models;

public class Team
{
    public List<User> Users { get; set; } = new List<User>();
    public short Position { get; set; } = 0;
    private byte ExplainerIndex { get; set; } = 0;

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public User GetExplainer()
    {
        return Users[ExplainerIndex];
    }

    public List<User> GetNonExplainers()
    {
        List<User> NonExplainers = Users;
        NonExplainers.RemoveAt(ExplainerIndex);
        return NonExplainers;
    }

    public void NextExplainer()
    {
        if (ExplainerIndex== Users.Count() - 1) ExplainerIndex= 0;
        else ExplainerIndex++;
    }
} // class end
