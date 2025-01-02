using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Models.DTOs
{
    public class SearchResultDto
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("incomplete_results")]
        public bool IncompleteResults { get; set; }

        [JsonProperty("items")]
        public List<RepositoryDto> Items { get; set; }
    }
}
