using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcommerceApi.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EcommerceApi.Core.Entities.Identity;

namespace EcommerceApi.Core.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly string _env;
        public TokenService(IConfiguration config)
        {
           this._config = config; 

           this._env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
           var key = (_env == "Development") ?_config["Token:Key"] : Environment.GetEnvironmentVariable("Token:Key");
           this._key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(key));
        }
        public string CreateToken(ECUser user)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName,user.DisplayName)
            };

            var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);

            var issuer = (_env == "Development") ?_config["Token:Issuer"] : Environment.GetEnvironmentVariable("Token:Issuer");

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenstring= tokenHandler.WriteToken(token);
            return tokenstring;
        }
    }
}