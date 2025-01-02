using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.Entities
{
    public class BookmarkEntity
    {
        [Key, Column(Order = 0)]
        public long UserId { get; set; }

        [Key, Column(Order = 1)]
        public long RepositoryId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual UserEntity User { get; set; }

        public virtual RepositoryEntity Repository { get; set; }
    }
}
