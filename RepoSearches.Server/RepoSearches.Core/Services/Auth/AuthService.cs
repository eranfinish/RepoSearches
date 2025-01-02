using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepoSearches.Models.DTOs;
using RepoSearches.Models.Entities;
using RepoSearches.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using RepoSearches.DAL;
using Microsoft.EntityFrameworkCore;

namespace RepoSearches.Core.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AuthService(AppDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public async Task<string> RegisterAsync(UserDto userDto)
        {
            var existingUser = await _context.Users
                .SingleOrDefaultAsync(u => u.UserName == userDto.UserName || u.Email == userDto.Email);

            if (existingUser != null)
                return "";

            var user = new UserEntity
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                //Password = userDto.Password,
                Password = HashPassword(userDto.Password),
               
                Name = userDto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            user.JwtToken = GenerateJwtToken(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.JwtToken;
        }

        public async Task<UserEntity> LoginAsync(LoginDto loginDto)
        {
            var jwtToken = string.Empty;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);

            if (user == null || !VerifyPassword(loginDto.Password, user.Password))
                return null;

            jwtToken = GenerateJwtToken(user);
            user.JwtToken = jwtToken;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            return user;
        }

       
        private string GenerateJwtToken(UserEntity user)
        {
            var jwtSettings = _jwtSettings.Value;
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpiryInMinutes),
                Issuer = jwtSettings.Issuer,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
