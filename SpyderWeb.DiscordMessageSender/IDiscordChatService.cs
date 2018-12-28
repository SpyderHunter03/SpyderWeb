using System.Threading.Tasks;

namespace SpyderWeb.DiscordMessageSender
{
    public interface IDiscordChatService
    {
        Task LogMessageToChannelAsync(string message, string channelName);
    }
}
