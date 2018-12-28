using Discord.Commands;
using System.Threading.Tasks;

namespace SpyderWeb.TagService
{
    public interface ITagService
    {
        ModuleInfo Module { get; }
        Task BuildTagsAsync();
    }
}
