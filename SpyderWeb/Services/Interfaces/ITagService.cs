using Discord.Commands;
using System.Threading.Tasks;

namespace SpyderWeb.Services.Interfaces
{
    public interface ITagService
    {
        ModuleInfo Module { get; }
        Task BuildTagsAsync();
    }
}