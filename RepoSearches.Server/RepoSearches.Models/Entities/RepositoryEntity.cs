using RepoSearches.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepoSearches.Models.Entities
{
    public class RepositoryEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string FullName { get; set; }

        public string HtmlUrl { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public int StargazersCount { get; set; }

        public int ForksCount { get; set; }

        public int OpenIssuesCount { get; set; }

        // Foreign key for Owner
        public long OwnerId { get; set; }

        public virtual OwnerEntity Owner { get; set; }
        public virtual ICollection<BookmarkEntity> Bookmarks { get; set; } = new List<BookmarkEntity>();

        // Navigation property for many-to-many relationship
        // public virtual ICollection<RepositoryTopicEntity> RepositoryTopics { get; set; } = new List<RepositoryTopicEntity>();
    }
}
