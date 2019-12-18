using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.JWTProvider.Services;
using Noteloves_server.Messages.Requests.Partner;
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

        // GET : api/partner
        [HttpGet]
        public IActionResult GetInfoPartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok("ADASD");
        }

        // POST : api/partner
        [HttpPost]
        public IActionResult AddPartner([FromForm] AddInfoPartner addInfoPartner)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok("ADASD");
        }

        // PATCH : api/partner/changename
        [HttpPatch]
        [Route("changename")]
        public async Task<IActionResult> ChangeNamePartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully added!"));
        }

        // PATCH : api/partner/changebirthday
        [HttpPatch]
        [Route("changebirthday")]
        public IActionResult ChangeBirthdayPartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok("ADASD");
        }

        // PATCH : api/partner/avatar
        [HttpPatch]
        [Route("avatar")]
        public IActionResult ChangeAvatarPartner()
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok("ADASD");
        }

        private int GetIdByToken(PartnerController partnerController)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            return _jWTService.GetIdByToken(accessToken);
        }
    }
}