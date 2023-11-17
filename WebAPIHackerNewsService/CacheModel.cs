namespace WebAPIHackerNewsService
{
    public class CacheModel
    {
        public IEnumerable<StoryDetails>? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
