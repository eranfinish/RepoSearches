using RepoSearches.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace RepoSearches.Models.Entities
{
    public class UserEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }

        [Required]//stored securely (hashed and salted)
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string JwtToken { get; set; }

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for Bookmarks
        public virtual ICollection<BookmarkEntity> Bookmarks { get; set; } = new List<BookmarkEntity>();
    }
}
