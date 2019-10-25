using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Requests
{
    public class LoginForm
    {
        public string email { get; set; }
        public string password { get; set; }
        public LoginForm(string str_email, string str_password)
        {
            email = str_email;
            password = str_password;
        }
    }
}
