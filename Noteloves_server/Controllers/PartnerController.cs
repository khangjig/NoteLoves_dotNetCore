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
    public class PartnerController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IPartnerService _partnerService;
        private IUserService _userService;
        private IJWTService _jWTService;

        public PartnerController(DatabaseContext context, IPartnerService partnerService, IUserService userService, IJWTService jWTService)
        {
            _context = context;
            _partnerService = partnerService;
            _userService = userService;
            _jWTService = jWTService;
        }

        // GET : api/partner/name
        [HttpGet]
        [Route("name")]
        public IActionResult GetNamePartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok(new DataResponse("200", _partnerService.GetNamePartner(userId), "Successfully!"));
        }

        // GET : api/partner/avatar
        [HttpGet]
        [Route("avatar")]
        public IActionResult GetAvatarPartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok(new DataResponse("200", Convert.ToBase64String(_partnerService.GetAvatarPartner(userId)), "Successfully!"));
        }


        // POST : api/partner/change-name
        [HttpPatch]
        [Route("change-name")]
        public IActionResult ChangeBirthdayPartner([FromForm] string name)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            _partnerService.ChangeNamePartner(userId, name);

            return Ok(new Response("200", "Successfully Updated!"));
        }

        // POST : api/partner/avatar
        [HttpPost]
        [Route("change-avatar")]
        public IActionResult ChangeAvatarPartner(IFormFile avatar)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            _partnerService.ChangeAvatarPartner(userId, avatar);

            return Ok(new Response("200", "Successfully Updated!"));
        }

        private int GetIdByToken(PartnerController partnerController)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            return _jWTService.GetIdByToken(accessToken);
        }
    }
}