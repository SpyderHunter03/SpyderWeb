using Discord;
using Newtonsoft.Json;
using SpyderWeb.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpyderWeb.EmojiTools
{
    public class EmojiService : IEmojiService
    {
        private Dictionary<string, string> emojiDictionary;
        private readonly string _emojiFileName = "emojis.json";

        public EmojiService()
        {
            BuildEmojiDictionary();
        }

        private void BuildEmojiDictionary()
        {
            if (emojiDictionary == null || emojiDictionary.Count == 0)
            {
                emojiDictionary = new Dictionary<string, string>();
                var emojiFileObject = ReadEmojiFile(_emojiFileName);

                emojiFileObject.People.ForEach(p => AddListToDictionary(p));
                emojiFileObject.Nature.ForEach(n => AddListToDictionary(n));
                emojiFileObject.Food.ForEach(f => AddListToDictionary(f));
                emojiFileObject.Activity.ForEach(a => AddListToDictionary(a));
                emojiFileObject.Travel.ForEach(t => AddListToDictionary(t));
                emojiFileObject.Objects.ForEach(o => AddListToDictionary(o));
                emojiFileObject.Symbols.ForEach(s => AddListToDictionary(s));
                emojiFileObject.Flags.ForEach(f => AddListToDictionary(f));
            }
        }

        private EmojiFileObject ReadEmojiFile(string emojiFileName)
        {
            var emojiFileObject = JsonConvert.DeserializeObject<EmojiFileObject>
                (File.ReadAllText(Path.Combine(SolutionHelper.GetConfigRoot(), emojiFileName)));
            return emojiFileObject;
        }

        private void AddListToDictionary(IEmoji emojis)
        {
            emojis.Names.ForEach(name => emojiDictionary.Add(name, emojis.Surrogates));
        }

        public string GetShorthand(Emoji emoji)
        {
            BuildEmojiDictionary();

            var key = emojiDictionary.FirstOrDefault(x => x.Value == emoji.Name).Key;
            if (string.IsNullOrEmpty(key))
                throw new Exception($"Could not find an emoji with value '{emoji.Name}'");
            return string.Concat(":", key, ":");
        }

        public bool TryGetShorthand(Emoji emoji, out string shorthand)
        {
            try
            {
                shorthand = GetShorthand(emoji);
                return true;
            }
            catch(Exception)
            {
                shorthand = "";
                return false;
            }
        }

        public Emoji GetEmojiFromText(string text)
        {
            BuildEmojiDictionary();

            if (!string.IsNullOrEmpty(text))
            {
                return emojiDictionary.FromText(text);
            }
            return emojiDictionary.FromText("x");
        }
    }
}
