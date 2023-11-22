using WebAPIHackerNewsService.Model;

namespace WebAPIHackerNewsService.Mapper
{
    public static class HackerNewsDTOMapper
    {
        public static List<HackerNewsDTO> mapToDTO(this StoryDetails[] storyDetails)
        {
            return storyDetails.Select(storyDetails => new HackerNewsDTO
            {
                CommentCount = storyDetails.CommentCount,
                PostedBy = storyDetails.PostedBy,
                Score = storyDetails.Score,
                Time = DateTimeOffset.FromUnixTimeSeconds(storyDetails.Time).DateTime,
                Title = storyDetails.Title,
                Uri = storyDetails.Uri
            }).ToList();

        }
    }
}
