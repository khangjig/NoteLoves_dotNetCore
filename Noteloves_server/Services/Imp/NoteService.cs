using Noteloves_server.Data;
using Noteloves_server.Messages.Requests.Note;
using Noteloves_server.Messages.Responses.Note;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class NoteService : INoteService
    {
        private readonly DatabaseContext _context;
        private INoteImageService _noteImageService;

        public NoteService(DatabaseContext context, INoteImageService noteImageService)
        {
            _context = context;
            _noteImageService = noteImageService;
        }

        public void AddNote(int userId, AddNoteForm addNoteForm)
        {
            Note note = new Note();
            note.UserId = userId;
            note.Title = addNoteForm.Title;
            note.Content = addNoteForm.Content;
            note.Anniversary = addNoteForm.Anniversary;
            note.Hidden = addNoteForm.Hidden;

            _context.Add(note);
        }

        public void UpdateNote(UpdateNoteForm updateNoteForm)
        {
            var note = _context.notes.First(x => x.Id == updateNoteForm.Id);
            note.Title = updateNoteForm.Title;
            note.Content = updateNoteForm.Content;
            note.Anniversary = updateNoteForm.Anniversary;
            note.Hidden = updateNoteForm.Hidden;
            note.UpdatedAt = DateTime.Now;

            _context.SaveChanges();

        }

        public void DeleteNote(int id)
        {
            var note = _context.notes.First(x => x.Id == id);
            _context.Remove(note);

            _context.SaveChanges();
        }

        public Note GetNoteById(int noteId)
        {
            return _context.notes.First(x => x.Id == noteId);
        }

        public List<NoteDataResponse> GetListNote(int userId, int page, int size)
        {
            //return _context.notes
            //            .Where(x => x.UserId == userId)
            //            .OrderByDescending(x => x.CreatedAt)
            //            .Skip((page-1)*size)
            //            .Take(size)
            //            .ToList();

            List < Note > listNotes = _context.notes
                                    .Where(x => x.UserId == userId)
                                    .OrderByDescending(x => x.CreatedAt)
                                    .Skip((page - 1) * size)
                                    .Take(size)
                                    .ToList();

            List<NoteDataResponse> listNoteDataResponses = new List<NoteDataResponse>();
            foreach (Note note in listNotes)
            {
                if(_noteImageService.CheckExistImage(note.Id))
                {
                    var firstImage = Convert.ToBase64String(_noteImageService.GetFirstImage(note.Id));
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, firstImage));
                }
                else
                {
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden));
                }

            }

            return listNoteDataResponses;
        }

        public bool CheckTitle(string title)
        {
            return _context.notes.Any(e => e.Title == title);
        }

        public bool CheckNoteByUser(int userId, int noteId)
        {
            return _context.notes.Any(e => e.UserId == userId && e.Id ==noteId);
        }

        public int GetNoteIdByTitle(string title)
        {
            var note = _context.notes.First(x => x.Title == title);
            return note.Id;
        }

        public bool CheckNoteExist(int noteId)
        {
            return _context.notes.Any(e => e.Id == noteId);
        }
    }
}
