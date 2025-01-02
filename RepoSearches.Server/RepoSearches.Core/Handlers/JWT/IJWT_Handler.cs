using RepoSearches.Models.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoSearches.Core.Handlers.JWT
{
    public interface IJWT_Handler
    {
        string GenerateJWT(UserEntity usr, IConfiguration config);
    }
}
