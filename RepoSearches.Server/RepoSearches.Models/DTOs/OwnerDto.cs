using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.DTOs
{
    public class OwnerDto
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }
    }
}
