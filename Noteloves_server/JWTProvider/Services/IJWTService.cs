using Noteloves_server.Messages.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Noteloves_server.JWTProvider.Services
{
    public interface IJWTService
    {
        string GenerateToken(string email);
        string GenerateRefreshToken();
        bool CheckAccount(LoginForm loginRequest);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
