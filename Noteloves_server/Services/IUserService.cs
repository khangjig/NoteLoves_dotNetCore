using Noteloves_server.Messages.Requests;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface IUserService
    {
        void UpdateRefreshToken(int id, string refreshToken);
        int GetIdByEmail(string email);
        string GetRefreshToken(string email);
    }
}
