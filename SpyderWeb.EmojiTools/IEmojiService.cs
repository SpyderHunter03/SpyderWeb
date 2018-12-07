using Discord;

namespace SpyderWeb.EmojiTools
{
    public interface IEmojiService
    {
        string GetShorthand(Emoji emoji);
        bool TryGetShorthand(Emoji emoji, out string shorthand);
        Emoji GetEmojiFromText(string text);
    }
}
