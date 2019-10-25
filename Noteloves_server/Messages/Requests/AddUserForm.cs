using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Requests
{
    public class AddUserForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
