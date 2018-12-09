using System.Threading.Tasks;

namespace SpyderWeb.Services.Interfaces
{
    public interface IDiscordChatService
    {
        Task LogMessageToChannelAsync(string message, string channelName);
    }
}