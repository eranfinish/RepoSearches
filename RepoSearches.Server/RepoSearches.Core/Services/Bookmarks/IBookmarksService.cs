using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepoSearches.Models.DTOs;

namespace RepoSearches.Core.Services.Bookmarks
{
    public interface IBookmarksService
    {
        Task<Object[]> AddBookmarkAsync(long owner, RepositoryDto repository);

        Task<object[]> GetBookmarksAsync(long userId);

        Task<Object[]> RemoveBookmarkAsync(long userId, long repositoryId);
        //public bool IsBookmarked(string owner, string repository)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
