using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RepoSearches.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoSearches.Models.Entities;

namespace RepoSearches.Core.Handlers.JWT
{
    public class JWT_Handler: IJWT_Handler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JWT_Handler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public string GenerateJWT(UserEntity usr, IConfiguration config)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    usr.Name = usr.Name ?? "";
        //    // JWT token generation
        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, usr.Email),
        //        new Claim(JwtRegisteredClaimNames.UniqueName, usr.UserName),
        //        new Claim("name", usr.Name),
        //        new Claim("id", usr.Id.ToString())
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: config["Jwt:Issuer"],
        //        audience: config["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddDays(1),//expires in one day
        //        signingCredentials: creds
        //    );
        //    var tokenString = tokenHandler.WriteToken(token);
        //    //_httpContextAccessor.HttpContext.Response.Cookies.Append("jwt", tokenString, new CookieOptions
        //    //{
        //    //    HttpOnly = true,
        //    //    Secure = false,
        //    //    SameSite = SameSiteMode.None
        //    //});

        //    return tokenString;
        //}
     
    }


}
