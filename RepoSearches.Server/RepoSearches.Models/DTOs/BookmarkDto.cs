namespace RepoSearches.Models.DTOs
{
    public class BookmarkDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty; // Link to authenticated user (from JWT or session)
        public long RepositoryId { get; set; }
        public string HtmlUrl {get; set; } = string.Empty;
        public string RepositoryName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string OwnerLogin { get; set; } = string.Empty;
        public DateTime BookmarkDate { get; set; }

    }
}
