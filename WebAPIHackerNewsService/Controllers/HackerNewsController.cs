using Microsoft.AspNetCore.Mvc;
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


        [HttpGet("best-stories/{storyNumber}")]
        [ResponseCache(Duration = 300)] // Cache response for 5 minutes
        public async Task<IActionResult> GetBestStories(int storyNumber)
        {
            try
            {
                var bestStories = await _hackerNewsService.GetBestStoriesAsync(storyNumber);
                return Ok(bestStories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
     }
}

       