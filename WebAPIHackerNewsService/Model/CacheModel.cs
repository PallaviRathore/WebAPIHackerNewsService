namespace WebAPIHackerNewsService.Model
{
    public class CacheModel
    {
        public IEnumerable<HackerNewsDTO>? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
