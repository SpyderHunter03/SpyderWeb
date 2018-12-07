using Discord;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpyderWeb.EmojiTools
{
    public static class EmojiExtensions
    {
        /// <summary>
        /// Return a Unicode Emoji given a shorthand alias
        /// </summary>
        /// <param name="text">A shorthand alias for the emoji, e.g. :race_car:</param>
        /// <returns>A unicode emoji, for direct use in a reaction or message.</returns>
        internal static Emoji FromText(this Dictionary<string, string> emojis, string text)
        {
            text = text.Trim(':');

            if (emojis.TryGetValue(text, out string unicode))
                return new Emoji(unicode);
            throw new ArgumentException($"The given alias ({text}) could not be matched to a Unicode Emoji.", nameof(text));
        }
    }
}
