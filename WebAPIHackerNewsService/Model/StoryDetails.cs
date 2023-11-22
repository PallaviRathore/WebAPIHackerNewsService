
using System.Text.Json.Serialization;

namespace WebAPIHackerNewsService.Model
{
    public class StoryDetails
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Uri { get; set; }

        [JsonPropertyName("by")]
        public string? PostedBy { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("descendants")]
        public int CommentCount { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

    }
}
