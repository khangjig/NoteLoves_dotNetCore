using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.JWTProvider.Services;
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
        private IJWTService _jWTService;

        public AvatarController(DatabaseContext context, IAvatarService avatarService, IUserService userService, IJWTService jWTService)
        {
            _context = context;
            _avatarService = avatarService;
            _userService = userService;
            _jWTService = jWTService;
    }

        // GET: api/Avatar?token =...
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAvatarByParamToken([FromQuery] string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (token == null)
            {
                return BadRequest(new Response("400", "Parameters is not invalid!"));
            }

            try
            {
                var userId = _jWTService.GetIdByToken(token);

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
            catch
            {
                return BadRequest(new Response("400", "Parameters is not invalid!"));
            }

        }

        // POST: api/Avatar/updated
        [HttpPost]
        [Route("updated")]
        public IActionResult UpdateAvatar(IFormFile avatar)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            var id = _jWTService.GetIdByToken(accessToken);

            if (avatar == null || avatar.Length == 0)
            {
                return BadRequest(new Response("400", "No Image"));
            }

            if (!_userService.UserExistsById(id))
            {
                return NotFound(new Response("404", "User not exist!"));
            }

            _avatarService.UpdateAvatar(id, avatar);

            return Ok(new Response("200", "Successfully Updated!"));
        }
    }
}