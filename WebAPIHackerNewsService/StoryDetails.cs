
using System.Text.Json.Serialization;

namespace WebAPIHackerNewsService
{
    public class StoryDetails
    {
        long time;

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

       
        public long Time { get; set; } // Assuming time is represented as Unix timestamp

        [JsonPropertyName("time ")]
        public DateTime TimeText
        {
            get { return DateTimeOffset.FromUnixTimeSeconds(Time).UtcDateTime; }
        }
    }
}
