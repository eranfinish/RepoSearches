using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RepoSearches.DAL;
using RepoSearches.Models.DTOs;
using RepoSearches.Models.Entities;
using Tweetinvi.Core.Extensions;

namespace RepoSearches.Core.Services.Bookmarks
{
    public class BookmarksService : IBookmarksService
    {
        private readonly AppDbContext _context;

        public BookmarksService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Object[]> AddBookmarkAsync(long userId, RepositoryDto repositoryDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check if repository exists
                var repository = await _context.Repositories
                    .Include(r => r.Owner)
                    .FirstOrDefaultAsync(r => r.Id == repositoryDto.Id);

                if (repository == null)
                {
                    // Map RepositoryEntity and OwnerEntity
                    var owner = await _context.Owners.FindAsync(repositoryDto.Owner.Id);
                    if (owner == null)
                    {
                        owner = new OwnerEntity
                        {                            
                            Login = repositoryDto.Owner.Login,
                            AvatarUrl = repositoryDto.Owner.AvatarUrl,
                            HtmlUrl = repositoryDto.Owner.HtmlUrl
                        };
                        _context.Owners.Add(owner);
                    }

                    repository = new RepositoryEntity
                    {                       
                        Name = repositoryDto.Name,
                        FullName = repositoryDto.FullName,
                        HtmlUrl = repositoryDto.HtmlUrl,
                        Description = repositoryDto.Description,
                        Language = repositoryDto.Language,
                        StargazersCount = repositoryDto.StargazersCount,
                        ForksCount = repositoryDto.ForksCount,
                        OpenIssuesCount = repositoryDto.OpenIssuesCount,
                        OwnerId = owner.Id,
                        Owner = owner
                    };
                    _context.Repositories.Add(repository);

                    _context.Entry(repository).State = EntityState.Added;
                    _context.Entry(repository.Owner).State = EntityState.Added;

                }

                // Check if bookmark exists
                var existingBookmark = await _context.Bookmarks
 .FirstOrDefaultAsync(b => b.UserId == userId && b.RepositoryId == repositoryDto.Id);


                if (existingBookmark == null)
                {
                    _context.Bookmarks.Add(new BookmarkEntity
                    {
                        UserId = userId,
                        RepositoryId = repositoryDto.Id,
                        CreatedAt = DateTime.UtcNow,
                        Repository = repository
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetBookmarksAsync(userId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error adding bookmark", ex);
            }
        }


      
            



         public async Task<Object[]> GetBookmarksAsync(long userId)
        {
          Object[] res;
            try { 
            // Fetch bookmarks with the associated repository and owner information
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == userId)
                .Include(b => b.Repository)            // Include the Repository details
                .ThenInclude(r => r.Owner)             // Include the Repository's Owner details
                .ToArrayAsync();                        // Get the results as a list

            // Convert to DTO array to return
            res = bookmarks.Select(b => new BookmarkDto
            {
                UserId = b.UserId,
                RepositoryId = b.RepositoryId,
                CreatedAt = b.CreatedAt,
                Repository = new RepositoryDto(){
                   Id = b.Repository.Id,
                   Name = b.Repository.Name,
                   FullName = b.Repository.FullName,
                   Owner = new OwnerDto()
                   {
                       AvatarUrl = b.Repository.Owner.AvatarUrl,
                       HtmlUrl = b.Repository.Owner.HtmlUrl,
                       Id = b.Repository.Owner.Id,
                       Login = b.Repository.Owner.Login
                   },
                   HtmlUrl = b.Repository.HtmlUrl,
                   Description = b.Repository.Description,
                   Language = b.Repository.Language,
                   ForksCount = b.Repository.ForksCount  ,
                   Bookmarked = true
                },
                
                //RepositoryName = b.Repository.Name,
            //    FullName = b.Repository.FullName,
            //    HtmlUrl = b.Repository.HtmlUrl,
            //    // Null check with ternary operator to handle possible null owner
            //    OwnerLogin = b.Repository.Owner != null ? b.Repository.Owner.Login : "Unknown"
                
            }).ToArray<Object>();

                return res;


          
            }
            catch (Exception ex)
            {

                throw new Exception("Error getting bookmarks", ex);
            }
        }


        public async Task<Object[]> RemoveBookmarkAsync(long userId, long repositoryId)
        {
            try
            {
                // Find the bookmark
                var bookmark = await _context.Bookmarks
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.RepositoryId == repositoryId);

                if (bookmark == null)
                {
                    throw new InvalidOperationException("Bookmark not found.");
                }

                // Remove the bookmark


                // Check if the repository is still bookmarked
                bool isExists =  await _context.Repositories.AnyAsync(r => r.Id == repositoryId);
                if (isExists)
                {
                    var repository = await _context.Repositories.FindAsync(repositoryId);
                    if (repository != null)
                    {
                        _context.Repositories.Remove(repository);
                    }
                }
   _context.Bookmarks.Remove(bookmark);
                await _context.SaveChangesAsync(); // Ensure this runs within a valid scope
                return await GetBookmarksAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing bookmark", ex);
            }
        }

    }
}
