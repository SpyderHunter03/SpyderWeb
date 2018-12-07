using System.Threading.Tasks;

namespace SpyderWeb.Twitch
{
    public interface ITwitchBot
    {
        Task InitializeAsync(string userName, string oauthToken, string channel);
    }
}
