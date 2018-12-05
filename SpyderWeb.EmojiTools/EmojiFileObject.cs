using Newtonsoft.Json;
using System.Collections.Generic;

namespace SpyderWeb.EmojiTools
{
    public class EmojiFileObject
    {
        [JsonProperty("people")]
        public List<Person> People { get; set; }
        [JsonProperty("nature")]
        public List<Nature> Nature { get; set; }
        [JsonProperty("food")]
        public List<Food> Food { get; set; }
        [JsonProperty("activity")]
        public List<Activity> Activity { get; set; }
        [JsonProperty("travel")]
        public List<Travel> Travel { get; set; }
        [JsonProperty("objects")]
        public List<Object> Objects { get; set; }
        [JsonProperty("symbols")]
        public List<Symbol> Symbols { get; set; }
        [JsonProperty("flags")]
        public List<Flag> Flags { get; set; }
    }

    public class Person : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
        [JsonProperty("hasDiversity")]
        public bool? HasDiversity { get; set; }
    }

    public class Nature : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public class Food : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public class Activity : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
        [JsonProperty("hasDiversity")]
        public bool? HasDiversity { get; set; }
    }

    public class Travel : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public class Object : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public class Symbol : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public class Flag : IEmoji
    {
        [JsonProperty("names")]
        public List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        public string Surrogates { get; set; }
    }

    public interface IEmoji
    {
        [JsonProperty("names")]
        List<string> Names { get; set; }
        [JsonProperty("surrogates")]
        string Surrogates { get; set; }
    }
}
