
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Noteloves_server.Data;
using Noteloves_server.JWTProvider.Services;
using Noteloves_server.Messages.Requests.Note;
using Noteloves_server.Messages.Responses;
using Noteloves_server.Messages.Responses.Note;
using Noteloves_server.Services;
using Noteloves_server.Services.Imp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private INoteService _noteService;
        private IUserService _userService;
        private INoteImageService _noteImageService;
        private IJWTService _jWTService;

        public NoteController(DatabaseContext context, INoteService noteService, IJWTService jWTService, IUserService userService, INoteImageService noteImageService)
        {
            _context = context;
            _noteService = noteService;
            _userService = userService;
            _noteImageService = noteImageService;
            _jWTService = jWTService;
        }

        // POST : api/note
        [HttpPost]
        public async Task<IActionResult> AddNoteByToken([FromForm] AddNoteForm addNoteForm, List<IFormFile> images)
        {
            var id = GetIdByToken(this);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_noteService.CheckTitle(addNoteForm.Title))
            {
                return BadRequest(new Response("400", "Title not invalid!"));
            }

            if (!_userService.UserExistsById(id))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            _noteService.AddNote(id, addNoteForm);
            await _context.SaveChangesAsync();

            _noteImageService.AddListImage(_noteService.GetNewestNote(id), images);
            await _context.SaveChangesAsync();

            return Ok(new Response("200", "Successfully added!"));
        }

        // GET : api/note
        [HttpGet]
        public IActionResult GetNoteInfomationByToken([FromForm] int id)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            if (!_noteService.CheckNoteByUser(userId, id) || !_noteService.CheckNoteExist(id))
            {
                return BadRequest(new Response("400", "The note does not exist!"));
            }

            var note = _noteService.GetNoteById(id);

            return Ok(new DataResponse("200", new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, null), "Successfully!"));
        }

        // GET : api/note/list?page=1&size=10
        [HttpGet]
        [Route("list")]
        public IActionResult GetListNotesByToken([FromQuery] int page, int size )
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            return Ok(new DataResponse("200", _noteService.GetListNote(userId, page, size), "Successfully!"));
        }

        // PUT : api/note
        [HttpPut]
        public IActionResult UpdateNoteByToken([FromForm] UpdateNoteForm updateNoteForm)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            if (_noteService.CheckTitle(updateNoteForm.Title))
            {
                return BadRequest(new Response("400", "Title is exist!"));
            }

            if (!_noteService.CheckNoteByUser(userId, updateNoteForm.Id) || !_noteService.CheckNoteExist(updateNoteForm.Id))
            {
                return BadRequest(new Response("400", "The note does not exist!"));
            }

            _noteService.UpdateNote(updateNoteForm);

            return Ok(new Response("200", "Successfully!"));
        }

        // DELETE : api/note
        [HttpDelete]
        public IActionResult DeleteNoteByToken([FromForm] int id)
        {
            var userId = GetIdByToken(this);

            if (!_userService.UserExistsById(userId))
            {
                return NotFound(new Response("404", "User not found!"));
            }

            if (!_noteService.CheckNoteByUser(userId, id) || !_noteService.CheckNoteExist(id))
            {
                return BadRequest(new Response("400", "The note does not exist!"));
            }

            _noteService.DeleteNote(id);

            return Ok(new Response("200", "Successfully!"));
        }

        private int GetIdByToken(NoteController noteController)
        {
            var authorization = Request.Headers["Authorization"];
            var accessToken = authorization.ToString().Replace("Bearer ", "");
            return _jWTService.GetIdByToken(accessToken);
        }
    }
}
