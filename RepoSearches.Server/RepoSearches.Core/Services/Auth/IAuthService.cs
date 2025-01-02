using RepoSearches.Models.DTOs;
using RepoSearches.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.TwitterEntities;

namespace RepoSearches.Core.Services.Auth
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(UserDto userDto);
        Task<UserEntity> LoginAsync(LoginDto loginDto);
    }
}
