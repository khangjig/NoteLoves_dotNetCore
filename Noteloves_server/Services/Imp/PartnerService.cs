using Microsoft.AspNetCore.Http;
using Noteloves_server.Data;
using Noteloves_server.Messages.Requests.Partner;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class PartnerService : IPartnerService
    {
        private readonly DatabaseContext _context;
        private IUserService _userService;

        public PartnerService(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public Partner GetPartner(int userID)
        {
            return _context.partner.FirstOrDefault(x => x.UserId == userID);
        }

       public void AddNew(int userID)
        {
            Partner newPartner = new Partner();

            newPartner.UserId = userID;
            newPartner.Name = "Partner Name";
            newPartner.Birthday = DateTime.Now;
            newPartner.Avatar = Encoding.UTF8.GetBytes("");

            _context.partner.Add(newPartner);
        }

        public string GetNamePartner(int userID)
        {
            if (_userService.CheckSync(userID))
            {
                return _context.users.First(x => x.PartnerId == userID).Name;
            }
            else
            {
                return _context.partner.FirstOrDefault(x => x.UserId == userID).Name;
            }
        }

        public byte[] GetAvatarPartner(int userID)
        {
            if (_userService.CheckSync(userID))
            {
                return _context.avatars.First(x => x.UserId == _userService.GetPartIDByUserID(userID)).Image;
            }
            else
            {
                return _context.partner.First(x => x.UserId == userID).Avatar;
            }
        }

        public void ChangeNamePartner(int userID, string Name)
        {
            var partner = _context.partner.First(x => x.UserId == userID);
            partner.Name = Name;

            _context.SaveChanges();
        }

        public void ChangeAvatarPartner(int userID, IFormFile image)
        {
            var partner = _context.partner.First(x => x.UserId == userID);
            partner.Avatar = EncodeImage(image);

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
