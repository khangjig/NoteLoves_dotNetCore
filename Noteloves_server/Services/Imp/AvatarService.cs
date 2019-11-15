using Microsoft.AspNetCore.Http;
using Noteloves_server.Data;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class AvatarService: IAvatarService
    {
        private readonly DatabaseContext _context;

        public AvatarService(DatabaseContext context)
        {
            _context = context;
        }

        public void CreateAvatar(int userId, byte[] image)
        {
            Avatar avatar = new Avatar();
            avatar.UserId = userId;
            avatar.Image = image;

            _context.avatars.Add(avatar);
        }

        public void UpdateAvatar(int userId, IFormFile image)
        {
            if (!_context.avatars.Any(e => e.UserId == userId))
            {
                CreateAvatar(userId, EncodeImage(image));
            }
            else
            {
                var avatar = _context.avatars.First(a => a.UserId == userId);
                avatar.Image = EncodeImage(image);
                avatar.UpdatedAt = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public byte[] GetAvatar(int userId)
        {
            var avatar = _context.avatars.First(a => a.UserId == userId);
            return avatar.Image;
        }

        public bool AvatarExistsByUserId(int UserId)
        {
            return _context.avatars.Any(e => e.UserId == UserId);
        }

        public byte[] EncodeImage(IFormFile image)
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
