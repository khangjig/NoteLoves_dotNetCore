using Microsoft.AspNetCore.Http;
using Noteloves_server.Messages.Requests.Partner;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface IPartnerService
    {
        Partner GetPartner(int UserID);
        void AddNew(int UserID);
        string GetNamePartner(int UserID);
        byte[] GetAvatarPartner(int UserID);
        void ChangeNamePartner(int UserID, string Name);
        void ChangeAvatarPartner(int UserID, IFormFile image);
    }
}
