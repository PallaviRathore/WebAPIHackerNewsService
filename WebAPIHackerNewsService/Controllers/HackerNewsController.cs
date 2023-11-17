using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Http;
using Newtonsoft.Json;

namespace WebAPIHackerNewsService.Controllers
{
    [ApiController]
    [Route("api/HackerNews")]
    public class HackerNewsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        public HackerNewsController(IHttpClientFactory httpClientFactory, IDistributedCache cache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
        }

        [HttpGet("best-stories/{n}")]
        public async Task<IActionResult> GetBestStories(int n)
        {
            try
            {
                var cacheKey = $"best-stories-{n}";

                // Attempt to retrieve data from the cache
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedStories = JsonConvert.DeserializeObject<IEnumerable<StoryDetails>>(cachedData);
                    return Ok(cachedStories);
                }

                // Fetch the list of best story IDs
                var bestStoryIds = await GetBestStoryIds();

                // Get details for the first n stories
                var bestStories = await GetBestStoriesDetails(bestStoryIds.Take(n));

                // Sort the stories by score in descending order
                bestStories = bestStories.OrderByDescending(s => s.Score).ToList();

                var newData = new CacheModel
                {
                    Data = bestStories,
                    Timestamp = DateTime.UtcNow
                };

                // Convert to JSON and store in the cache for a specified duration (e.g., 5 minutes)
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(bestStories), cacheOptions);

                return Ok(bestStories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<IEnumerable<int>> GetBestStoryIds()
        {
            var response = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            return JsonConvert.DeserializeObject<IEnumerable<int>>(response) ?? new List<int>();
        }

        private async Task<IEnumerable<StoryDetails>> GetBestStoriesDetails(IEnumerable<int> storyIds)
        {
            var tasks = storyIds.Select(async id =>
            {
                var response = await _httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
                return JsonConvert.DeserializeObject<StoryDetails?>(response);
            });

            return await Task.WhenAll(tasks);
        }
    }
}