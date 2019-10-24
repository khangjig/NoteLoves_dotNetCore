using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Responses
{
    public class LoginRespone
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginRespone(string accessToken, string refreshToken)
        {
            this.AccessToken = accessToken;
            this.RefreshToken = refreshToken;
        }
    }
}
