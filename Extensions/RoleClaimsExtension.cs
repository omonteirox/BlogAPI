﻿using Blog.Models;
using System.Security.Claims;

namespace BlogAPI.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));
            return claims;
        }
    }
}