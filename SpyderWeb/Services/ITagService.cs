using Discord.Commands;
using System.Threading.Tasks;

namespace SpyderWeb.Services
{
    public interface ITagService
    {
        ModuleInfo Module { get; }
        Task BuildTagsAsync();
    }
}