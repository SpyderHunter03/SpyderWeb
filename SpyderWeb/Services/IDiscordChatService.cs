using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface IDiscordChatService
    {
        Task LogMessageToChannelAsync(string message, string channelName);
    }
}