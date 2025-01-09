using Newtonsoft.Json;
using RepoSearches.Models.Entities;
//using System.Text.Json.Serialization;


namespace RepoSearches.Models.DTOs
{
    public class BookmarkDto
    {
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [JsonProperty("repositoryId")]
        public long RepositoryId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("isBookmarked")]
        public bool IsBookmarked { get; set; } = true;

        [JsonProperty("repository")]
        public RepositoryDto Repository { get; set; }
    }
}
