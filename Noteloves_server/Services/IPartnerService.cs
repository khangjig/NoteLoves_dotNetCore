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
        void AddInfoPartner(AddInfoPartner addInfoPartner);
        void ChangeNamePartner(int UserID, string Name);
        void ChangeAvatarPartner(int UserID, byte[] AvatarBase64);
        void ChangeBirthDayPartner(int UserID, DateTime birthday);
    }
}
