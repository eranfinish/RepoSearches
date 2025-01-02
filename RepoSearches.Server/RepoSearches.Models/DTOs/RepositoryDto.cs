using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.DTOs
{
    public class RepositoryDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("owner")]
        public OwnerDto Owner { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("stargazers_count")]
        public int StargazersCount { get; set; }

        [JsonProperty("forks_count")]
        public int ForksCount { get; set; }

        [JsonProperty("open_issues_count")]
        public int OpenIssuesCount { get; set; }

        [JsonProperty("topics")]
        public List<string> Topics { get; set; }
    }
}
