using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http;
using static Microsoft.Extensions.Logging.ILogger;

namespace WebAPIHackerNewsService.Logic
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<StoryDetails>> GetBestStoriesAsync(int n);
    }

    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<HackerNewsService> _logger;
       

        public HackerNewsService(IHttpClientFactory httpClientFactory, IDistributedCache cache,ILogger<HackerNewsService> log)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
            _logger = log;
        }

        public async Task<IEnumerable<StoryDetails>> GetBestStoriesAsync(int n)
        {
            try
            {
                var cacheKey = $"best-stories-{n}";

                // Attempt to retrieve data from the cache
                var cachedData = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    var cachedStories = JsonConvert.DeserializeObject<IEnumerable<StoryDetails>>(cachedData);
                    return cachedStories;
                }

               
                    // Double-check to avoid fetching data if it has been updated by another thread
                    cachedData = await _cache.GetStringAsync(cacheKey);
                    if (!string.IsNullOrEmpty(cachedData))
                    {
                        var cachedStories = JsonConvert.DeserializeObject<IEnumerable<StoryDetails>>(cachedData);
                        return cachedStories;
                    }
                    // Fetch the list of best story IDs
                    var bestStoryIds = await GetBestStoryIds();

                // Get details for the first n stories
                var bestStories = await GetBestStoriesDetails(bestStoryIds.Take(n), n);

                // Sort the stories by score in descending order
                bestStories = bestStories.OrderByDescending(s => s.Score).ToList();

                var newData = new CacheModel
                {
                    Data = bestStories,
                    Timestamp = DateTime.UtcNow
                };

                await StoreInCacheJSON(bestStories, cacheKey);
                return bestStories;
              
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting best stories.");
                throw;
            }
              
           
        }

        // Convert to JSON and store in the cache for a specified duration (e.g., 5 minutes)
        private async Task StoreInCacheJSON(IEnumerable<StoryDetails> bestStories, string cacheKey)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(bestStories), cacheOptions);
        }

        private async Task<IEnumerable<int>> GetBestStoryIds()
        {
            var response = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            return JsonConvert.DeserializeObject<IEnumerable<int>>(response) ?? new List<int>();
        }

        private async Task<List<StoryDetails>> GetBestStoriesDetails(IEnumerable<int> storyIds, int n)
        {
            var stories = new List<StoryDetails>();

            try
            {
                foreach (var storyId in storyIds.Take(n))
                {
                    var story = await _httpClient.GetFromJsonAsync<StoryDetails>($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
                    if (story != null)
                    {
                        stories.Add(story);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting best stories details.");
                throw;
            }
            return stories.OrderByDescending(s => s.Score).ToList();
        }
    }
         
        }
    

