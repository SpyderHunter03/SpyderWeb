using Discord;
using Discord.Commands;
using SpyderWeb.EmojiTools;
using System.Threading.Tasks;

namespace SpyderWeb.ModuleBase
{
    public class SpyderModuleBase : ModuleBase<SocketCommandContext>
    {
        private readonly IEmojiService _emojiService;

        public SpyderModuleBase(IEmojiService emojiService)
        {
            _emojiService = emojiService;
        }

        public Emoji TagNotFound
        {
            get
            {
                return _emojiService.GetEmojiFromText("mag_right");
            }
        }

        public Emoji Pass
        {
            get
            {
                return _emojiService.GetEmojiFromText("ok_hand");
            }
        }

        public Emoji Fail
        {
            get
            {
                return _emojiService.GetEmojiFromText("octagonal_sign");
            }
        }

        public Emoji Removed
        {
            get
            {
                return _emojiService.GetEmojiFromText("put_litter_in_its_place");
            }
        }

        public Task ReactAsync(IEmote emote) => Context.Message.AddReactionAsync(emote);
    }
}
