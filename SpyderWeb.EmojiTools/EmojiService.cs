using Newtonsoft.Json;
using SpyderWeb.Helpers;
using System.Collections.Generic;
using System.IO;

namespace SpyderWeb.EmojiTools
{
    public class EmojiService
    {
        private Dictionary<string, string> emojiDictionary;
        private readonly string emojiFileName = "emojis.json";

        public EmojiService() { }

        public void BuildEmojiDictionary()
        {
            var emojis = JsonConvert.DeserializeObject<EmojiFileObject>(File.ReadAllText(Path.Combine(SolutionHelper.GetConfigRoot(), emojiFileName)));
            emojiDictionary = new Dictionary<string, string>();
            
            emojis.People.ForEach(p => AddListToDictionary(p));
            emojis.Nature.ForEach(n => AddListToDictionary(n));
            emojis.Food.ForEach(f => AddListToDictionary(f));
            emojis.Activity.ForEach(a => AddListToDictionary(a));
            emojis.Travel.ForEach(t => AddListToDictionary(t));
            emojis.Objects.ForEach(o => AddListToDictionary(o));
            emojis.Symbols.ForEach(s => AddListToDictionary(s));
            emojis.Flags.ForEach(f => AddListToDictionary(f));

            EmojiMap.Map = emojiDictionary;
        }

        private void AddListToDictionary(IEmoji emojis)
        {
            foreach (var name in emojis.Names)
            {
                emojiDictionary.Add(name, emojis.Surrogates);
            }
        }
    }
}
