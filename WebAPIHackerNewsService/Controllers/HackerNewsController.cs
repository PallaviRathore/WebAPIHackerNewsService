using Microsoft.AspNetCore.Mvc;
using WebAPIHackerNewsService.Logic;

namespace WebAPIHackerNewsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }


        [HttpGet("best-stories/{n}")]
        [ResponseCache(Duration = 300)] // Cache response for 5 minutes
        public async Task<IActionResult> GetBestStories(int n)
        {
            try
            {
                var bestStories = await _hackerNewsService.GetBestStoriesAsync(n);
                return Ok(bestStories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
     }
}

       