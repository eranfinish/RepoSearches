﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using RepoSearches.DAL;
using RepoSearches.Models.DTOs;
using System.Security.Claims;
using RepoSearches.Core.Services.Bookmarks;

[ApiController]
[Route("api/bookmarks")]
[Authorize] // Ensure only authenticated users can access this controller
public class BookmarksController : ControllerBase
{
   // private readonly AppDbContext _context;
    private readonly IBookmarksService _bookmarksService;
    public BookmarksController(IBookmarksService bookmarksService)
    {
     //   _context = context;
        _bookmarksService = bookmarksService;
    }

    [Authorize]
    [HttpPost]//add Boomark
    public async Task<IActionResult> BookmarkRepository([FromBody] BookmarkDto bookmark)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        string userIdString;
        userIdString = getUserId();
        if (!long.TryParse(userIdString, out long userId))
            return Unauthorized();

        try
        {
            var bookmarks = await _bookmarksService.AddBookmarkAsync(userId, bookmark.Repository);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {   //simulate Log exception
            Console.WriteLine(ex.Message);

            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    private string getUserId()
    { string userIdString = string.Empty;
        var claims = User.Claims.ToList<Claim>();
        if (claims.Count > 1)       
        userIdString = claims[1].Value;
        return userIdString;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetBookmarkedRepositories()
    {
        string userIdString;
        userIdString = getUserId();
        if (!long.TryParse(userIdString, out long userId))
            return Unauthorized();

        try
        {
            var bookmarks =  await _bookmarksService.GetBookmarksAsync(userId);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        {
            //simulate Log exception
            Console.WriteLine(ex.Message);
           
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [Authorize]
    [HttpDelete("bookmarks/{userId}/{reposityId}")]
    public async  Task<IActionResult> RemoveBookmark(long userId, long reposityId)
    {
       // var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
      //  if (!long.TryParse(userIdString, out long userId))
       //     return Unauthorized();

        try
        {
            var bookmarks = await _bookmarksService.RemoveBookmarkAsync(userId, reposityId);
            return Ok(bookmarks);
        }
        catch (Exception ex)
        { 
            //simulate Log exception
            Console.WriteLine(ex.Message);
           
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }



}
