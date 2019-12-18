using Microsoft.AspNetCore.Http;
using Noteloves_server.Data;
using Noteloves_server.Messages.Requests.Partner;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class PartnerService : IPartnerService
    {
        private readonly DatabaseContext _context;

        public PartnerService(DatabaseContext context)
        {
            _context = context;
        }

        public Partner GetPartner(int userID)
        {
            return _context.partner.FirstOrDefault(x => x.UserId == userID);
        }

        public void AddInfoPartner(AddInfoPartner addInfoPartner)
        {
            Partner newPartner = new Partner();

            newPartner.Name = addInfoPartner.NameParter;
            newPartner.Birthday = addInfoPartner.BirthDayPartner;
            newPartner.Avatar = EncodeImage(addInfoPartner.AvatarPartner);

            _context.partner.Add(newPartner);
        }

        public void ChangeNamePartner(int userID, string Name)
        {
            var partner = _context.partner.First(x => x.UserId == userID);
            partner.Name = Name;

            _context.SaveChanges();
        }
        public void ChangeBirthDayPartner(int userID, DateTime birthday)
        {
            var partner = _context.partner.First(x => x.UserId == userID);
            partner.Birthday  = birthday;

            _context.SaveChanges();
        }

        public void ChangeAvatarPartner(int userID, byte[] AvatarBase64)
        {
            var partner = _context.partner.First(x => x.UserId == userID);
            partner.Avatar = AvatarBase64;

            _context.SaveChanges();
        }

        private byte[] EncodeImage(IFormFile image)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }
    }
}
