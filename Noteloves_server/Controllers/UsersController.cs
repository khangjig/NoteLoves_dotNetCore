using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Noteloves_server.Data;
using Noteloves_server.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using Noteloves_server.Messages.Requests;
using Noteloves_server.Services;
using Noteloves_server.Messages.Responses;

namespace Noteloves_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private IUserService _userService;

        public UsersController(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        
        // GET: api/Users
        [HttpGet]
        public IActionResult Getusers()
        {
            return Ok(new DataResponse("200", _userService.GetAllUser(), "Successfully!"));
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUserById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userService.GetInfomation(id);

            if (user == null)
            {
                return NotFound(new Response("404", "User not found!"));
            }

            user.RefreshToken = null;
            user.Password = null;

            return Ok(new DataResponse("200", user, "Successfully!"));
        }

        // PUT: api/Users/EditInfo
        [HttpPut("{EditInfo}")]
        public async Task<IActionResult> EditInformationUser([FromBody] EditUserForm editUserForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExistsById(editUserForm.Id))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            _userService.EidtInfomation(editUserForm);
            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully!"));
        }

        // PUT: api/Users/ChangePassword
        [HttpPatch("{ChangePassword}")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm changePasswordForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_userService.UserExistsById(changePasswordForm.Id))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            if (!_userService.CheckOldPassword(changePasswordForm.Id, changePasswordForm.OldPassword))
            {
                return BadRequest(new Response("400", "Old password not correct!"));
            }

            _userService.ChangePassword(changePasswordForm.Id, changePasswordForm.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully!"));
        }

        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] AddUserForm addUserForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_userService.UserExistsByEmail(addUserForm.Email))
            {
                return BadRequest(new Response("400","Email is not invalid"));
            }

            _userService.AddUser(addUserForm);
            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully added!"));
        }
    }
}