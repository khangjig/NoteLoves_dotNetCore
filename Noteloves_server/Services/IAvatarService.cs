using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface IAvatarService
    {
        void CreateAvatar(int userId, byte[] image);
        void UpdateAvatar(int userId, IFormFile image);
        byte[] GetAvatar(int userId);
        bool AvatarExistsByUserId(int UserId);
        byte[] EncodeImage(IFormFile image);
    }
}
