using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Requests.Partner
{
    public class AddInfoPartner
    {
        public string NameParter { get; set; }
        public DateTime BirthDayPartner { get; set; }
        public IFormFile AvatarPartner { get; set; }
    }
}
