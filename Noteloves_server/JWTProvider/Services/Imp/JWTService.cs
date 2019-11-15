using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Noteloves_server.Data;
using Noteloves_server.Helpers;
using Noteloves_server.Messages.Requests;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Noteloves_server.JWTProvider.Services
{
    public class JWTService : IJWTService
    {
        private readonly AppSettings _appSettings;
        private readonly DatabaseContext _context;

        public JWTService(IOptions<AppSettings> appSettings, DatabaseContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public bool CheckAccount(LoginForm loginRequest)
        {
            var user = _context.users
                .Where(u => u.Email == loginRequest.email && u.Password == loginRequest.password);

            if (user.Count() < 1)
                return false;

            return true;
        }

        public string GenerateToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        //new Claim("email", email)
                        new Claim(ClaimTypes.Name,"1"),
                        new Claim(ClaimTypes.Email,email)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(accessToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GetEmailByToken(string accessToken)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            return token.Claims.First(c => c.Type == "email").Value;
        }

        public int GetIdByToken(string accessToken)
        {
            var principal = GetPrincipalFromExpiredToken(accessToken);
            return Convert.ToInt32(principal.Identity.Name);
        }
    }
}
