// File: Controllers/GitHubController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RepoSearches.Models;
using RepoSearches.Models.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/github")]
public class GitHubController : ControllerBase
{
    private readonly string _searchRepositoriesUrl;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    public GitHubController(IHttpClientFactory httpClientFactory, IOptions<GitHubConfig> config)
    {
        _httpClientFactory = httpClientFactory;
        _searchRepositoriesUrl = config.Value.SearchRepositoriesUrl;
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<IActionResult> SearchRepositories([FromQuery] string q)
    {
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "YourAppName");
        https://api.github.com/search/repositories?q=
        var response = await client.GetStringAsync($"https://api.github.com/search/repositories?q={q}");
        //var response = await client.GetStringAsync($"{_searchRepositoriesUrl}{q}");
        var repositories = JsonConvert.DeserializeObject<SearchResultDto>(response);
        return Ok(repositories);
    }
}
