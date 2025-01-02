using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OwnerEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Login { get; set; }

        public string AvatarUrl { get; set; }

        public string HtmlUrl { get; set; }

        // Navigation property
        public virtual ICollection<RepositoryEntity> Repositories { get; set; } = new List<RepositoryEntity>();
    }
}
