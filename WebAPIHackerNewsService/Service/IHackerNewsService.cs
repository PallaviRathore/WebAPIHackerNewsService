using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http;
using WebAPIHackerNewsService.Model;
using static Microsoft.Extensions.Logging.ILogger;

namespace WebAPIHackerNewsService
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerNewsDTO>> GetBestStoriesAsync(int n);
    } 
}
    

