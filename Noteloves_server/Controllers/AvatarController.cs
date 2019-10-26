using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.Messages.Responses;
using Noteloves_server.Services;

namespace Noteloves_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IAvatarService _avatarService;
        private IUserService _userService;

        public AvatarController(DatabaseContext context, IAvatarService avatarService, IUserService userService)
        {
            _context = context;
            _avatarService = avatarService;
            _userService = userService;
    }

        // GET: api/Avatar/5
        [HttpGet("{userId}")]
        public IActionResult GetAvatarByUserId([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            if (!_avatarService.AvatarExistsByUserId(userId))
            {
                return NotFound(new Response("404", "User is not Avatar!"));
            }

            var avatar = _avatarService.GetAvatar(userId);
            MemoryStream image = new MemoryStream(avatar);

            return Ok(image);
        }

        // POST: api/Avatar/updated
        [HttpPost("{updated}")]
        public async Task<IActionResult> UpdateAvatar([FromForm] int userId, IFormFile avatar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest(new Response("400", "No Image"));
            }

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            _avatarService.UpdateAvatar(userId, avatar);
            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully Updated!"));
        }
    }
}