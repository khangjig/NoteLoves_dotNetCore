using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.JWTProvider.Services;
using Noteloves_server.Messages.Responses;
using Noteloves_server.Services;
using Noteloves_server.Services.Imp;

namespace Noteloves_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteImageController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private INoteService _noteService;
        private IUserService _userService;
        private INoteImageService _noteImageService;
        private IJWTService _jWTService;

        public NoteImageController(DatabaseContext context, INoteService noteService, IUserService userService, INoteImageService noteImageService, IJWTService jWTService)
        {
            _context = context;
            _noteService = noteService;
            _userService = userService;
            _noteImageService = noteImageService;
            _jWTService = jWTService;
        }

        // GET : api/noteimage/list
        [HttpGet]
        [Route("list")]
        public IActionResult GetListNoteImageByToken([FromQuery] int noteId)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            if ( !_noteService.CheckNoteExist(noteId))
            {
                return BadRequest(new Response("400", "The note does not exist!"));
            }
            
            if (!_noteService.CheckNoteByUser(userId, noteId) && !_noteService.CheckNoteByPartner(userId, noteId))
            {
                return BadRequest(new Response("400", "You do not have this note!"));
            }

            return Ok(new DataResponse("200", _noteImageService.GetListImage(noteId), "Successfully!"));
        }

        private int GetIdByToken(NoteImageController noteImageController)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            return _jWTService.GetIdByToken(accessToken);
        }
    }
}