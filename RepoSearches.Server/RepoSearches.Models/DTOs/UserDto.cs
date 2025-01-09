using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.DTOs
{
    public class UserDto: LoginDto    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("isRegistering")]
        public bool IsRegistering { get; set; }

        [JsonIgnore]
        public string Role { get; set; } = "User";

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
