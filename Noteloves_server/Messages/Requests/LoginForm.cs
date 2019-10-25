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
    }
}
