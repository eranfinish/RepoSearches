using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.DTOs
{
    public class UserDto: LoginDto    {        
        public long Id { get; set; }       
        public string Name { get; set; } = string.Empty;       
        public string Email { get; set; } = string.Empty; 
        public bool IsRegistering { get; set; } 
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
