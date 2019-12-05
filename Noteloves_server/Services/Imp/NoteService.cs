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
        private IUserService _userService;

        public NoteService(DatabaseContext context, INoteImageService noteImageService, IUserService userService)
        {
            _context = context;
            _noteImageService = noteImageService;
            _userService = userService;
        }

        public void AddNote(int userId, AddNoteForm addNoteForm)
        {
            Note note = new Note();
            note.UserId = userId;
            note.Title = addNoteForm.Title;
            note.Content = addNoteForm.Content;
            note.Anniversary = addNoteForm.Anniversary;
            note.Hidden = addNoteForm.Hidden;
            note.Alarm = addNoteForm.Alarm;

            _context.Add(note);
        }

        public void UpdateNote(UpdateNoteForm updateNoteForm)
        {
            var note = _context.notes.First(x => x.Id == updateNoteForm.Id);
            note.Title = updateNoteForm.Title;
            note.Content = updateNoteForm.Content;
            note.Anniversary = updateNoteForm.Anniversary;
            note.Hidden = updateNoteForm.Hidden;
            note.Alarm = updateNoteForm.Alarm;
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
            List<Note> listNotes;
            if (_userService.CheckSync(userId))
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userId)
                    .Union(_context.notes.Where(x => x.UserId == _userService.GetPartIDByUserID(userId) && x.Hidden == false))
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
            }
            else
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();
            }

            List<NoteDataResponse> listNoteDataResponses = new List<NoteDataResponse>();
            foreach (Note note in listNotes)
            {
                if(_noteImageService.CheckExistImage(note.Id))
                {
                    var firstImage = Convert.ToBase64String(_noteImageService.GetFirstImage(note.Id));
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId,firstImage));
                }
                else
                {
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId));
                }

            }

            return listNoteDataResponses;
        }

        public List<NoteDataResponse> GetNoteOnThisDay(int userID)
        {
            List<Note> listNotes;
            if (_userService.CheckSync(userID))
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userID
                            && x.Alarm == true
                            && x.Anniversary.Day == DateTime.Now.Day
                            && x.Anniversary.Month == DateTime.Now.Month)
                    .Union(_context.notes.Where(x => x.UserId == _userService.GetPartIDByUserID(userID)
                            && x.Alarm == true
                            && x.Hidden == false
                            && x.Anniversary.Day == DateTime.Now.Day
                            && x.Anniversary.Month == DateTime.Now.Month))
                    .OrderByDescending(x => x.Anniversary)
                    .Take(5)
                    .ToList();
            }
            else
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userID
                            && x.Alarm == true
                            && x.Anniversary.Day == DateTime.Now.Day
                            && x.Anniversary.Month == DateTime.Now.Month)
                    .OrderByDescending(x => x.Anniversary)
                    .Take(5)
                    .ToList();
            }

            List<NoteDataResponse> listNoteDataResponses = new List<NoteDataResponse>();
            foreach (Note note in listNotes)
            {
                if (_noteImageService.CheckExistImage(note.Id))
                {
                    var firstImage = Convert.ToBase64String(_noteImageService.GetFirstImage(note.Id));
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId, firstImage));
                }
                else
                {
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId));
                }

            }

            return listNoteDataResponses;
        }

        public List<NoteDataResponse> GetListNoteByWeek(int userID)
        {
            int days, months;
            List<Note> listNotes;

            DateTime DayNow = DateTime.Now;

            if ((DayNow.Day + 7) > DateTime.DaysInMonth(DayNow.Year, DayNow.Month))
            {
                if (_userService.CheckSync(userID))
                {
                    int PartID = _userService.GetPartIDByUserID(userID);
                    days = 7 - (DateTime.DaysInMonth(DayNow.Year, DayNow.Month) - DayNow.Day);
                    months = (DayNow.Month == 12) ? 1 : (DayNow.Month + 1);

                    listNotes = _context.notes
                        .Where(x => x.UserId == userID
                                    && x.Alarm == true
                                    && x.Anniversary.Day > DayNow.Day
                                    && x.Anniversary.Day <= (DayNow.Day + 7 - days)
                                    && x.Anniversary.Month == DayNow.Month)
                        .Union(_context.notes
                                .Where(x => x.UserId == userID
                                    && x.Alarm == true
                                    && x.Anniversary.Day >= 1
                                    && x.Anniversary.Day <= days
                                    && x.Anniversary.Month == months)
                            )
                        .Union(_context.notes
                                .Where(x => x.UserId == PartID
                                    && x.Alarm == true
                                    && x.Hidden == false
                                    && x.Anniversary.Day > DayNow.Day
                                    && x.Anniversary.Day <= (DayNow.Day + 7 - days)
                                    && x.Anniversary.Month == DayNow.Month)
                            )
                        .Union(_context.notes
                                .Where(x => x.UserId == PartID
                                    && x.Alarm == true
                                    && x.Hidden == false
                                    && x.Anniversary.Day >= 1
                                    && x.Anniversary.Day <= days
                                    && x.Anniversary.Month == months)
                            )
                        .OrderBy(x => x.Anniversary)
                        .Take(5)
                        .ToList();
                }
                else
                {
                    days = 7 - (DateTime.DaysInMonth(DayNow.Year, DayNow.Month) - DayNow.Day);
                    months = (DayNow.Month == 12) ? 1 : (DayNow.Month + 1);

                    listNotes = _context.notes
                        .Where(x => x.UserId == userID
                                    && x.Alarm == true
                                    && x.Anniversary.Day > DayNow.Day
                                    && x.Anniversary.Day <= (DayNow.Day + 7 - days)
                                    && x.Anniversary.Month == DayNow.Month)
                        .Union(_context.notes
                                .Where(x => x.UserId == userID
                                    && x.Alarm == true
                                    && x.Anniversary.Day >= 1
                                    && x.Anniversary.Day <= days
                                    && x.Anniversary.Month == months)
                            )
                        .OrderBy(x => x.Anniversary)
                        .Take(5)
                        .ToList();
                }
            }
            else
            {
                if (_userService.CheckSync(userID))
                {
                    listNotes = _context.notes
                        .Where(x => x.UserId == userID
                                && x.Alarm == true
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Day <= DateTime.Now.Day + 7
                                && x.Anniversary.Month == DateTime.Now.Month)
                        .Union(_context.notes.Where(x => x.UserId == _userService.GetPartIDByUserID(userID)
                                && x.Alarm == true
                                && x.Hidden == false
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Day <= DateTime.Now.Day + 7
                                && x.Anniversary.Month == DateTime.Now.Month))
                        .OrderByDescending(x => x.Anniversary)
                        .Take(5)
                        .ToList();
                }
                else
                {
                    listNotes = _context.notes
                        .Where(x => x.UserId == userID
                                && x.Alarm == true
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Day <= DateTime.Now.Day + 7
                                && x.Anniversary.Month == DateTime.Now.Month)
                        .OrderByDescending(x => x.Anniversary)
                        .Take(5)
                        .ToList();
                }
            }

            List<NoteDataResponse> listNoteDataResponses = new List<NoteDataResponse>();
            foreach (Note note in listNotes)
            {
                if (_noteImageService.CheckExistImage(note.Id))
                {
                    var firstImage = Convert.ToBase64String(_noteImageService.GetFirstImage(note.Id));
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId, firstImage));
                }
                else
                {
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId));
                }

            }

            return listNoteDataResponses;
        }

        public List<NoteDataResponse> GetListNoteInMonth(int userID)
        {
            List<Note> listNotes;
            if (_userService.CheckSync(userID))
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userID
                                && x.Alarm == true
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Month == DateTime.Now.Month)
                    .Union(_context.notes.Where(x => x.UserId == _userService.GetPartIDByUserID(userID)
                                && x.Alarm == true
                                && x.Hidden == false
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Month == DateTime.Now.Month))
                    .OrderBy(x => x.Anniversary)
                    .Take(5)
                    .ToList();
            }
            else
            {
                listNotes = _context.notes
                    .Where(x => x.UserId == userID
                                && x.Alarm == true
                                && x.Anniversary.Day > DateTime.Now.Day
                                && x.Anniversary.Month == DateTime.Now.Month)
                    .OrderBy(x => x.Anniversary)
                    .Take(5)
                    .ToList();
            }

            List<NoteDataResponse> listNoteDataResponses = new List<NoteDataResponse>();
            foreach (Note note in listNotes)
            {
                if (_noteImageService.CheckExistImage(note.Id))
                {
                    var firstImage = Convert.ToBase64String(_noteImageService.GetFirstImage(note.Id));
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId, firstImage));
                }
                else
                {
                    listNoteDataResponses.Add(new NoteDataResponse(note.Id, note.Title, note.Content, note.Anniversary, note.Hidden, note.UserId));
                }

            }

            return listNoteDataResponses;
        }

        public bool CheckNoteByUser(int userId, int noteId)
        {
            return _context.notes.Any(e => e.UserId == userId && e.Id == noteId);
        }

        public int GetUserIDByNoteID(int noteId)
        {
            return _context.notes.First(e => e.Id == noteId).UserId;
        }

        public int GetPartnerIDByNoteID(int noteID)
        {
            var value = _context.users.Join(_context.notes.Where(x => x.Id == noteID),
                    user => user.Id, 
                    note => note.UserId,
                    (user, note) => new {
                                user.PartnerId
                            }).FirstOrDefault();

            return value.PartnerId;
        }

        public bool CheckNoteByPartner(int partnerId, int noteId)
        {
            // var check = _context.notes.Any(e => e.Id == noteId && GetPartnerIDByNoteID(noteId) == partnerId) // Cach1

            var check = _context.notes.Where(e => e.Id == noteId)
                .Join(_context.users.Where(x => x.PartnerId == partnerId),
                note => note.UserId, 
                user => user.Id,
                (user,note) => note
                ).Any(); // Cach2

            return check;
        }

        public int GetNewestNote(int UserID)
        {
            var note = _context.notes.Where(x=>x.UserId== UserID).OrderByDescending(x=>x.CreatedAt).Take(1).ToList();
            return note[0].Id;
        }

        public bool CheckNoteExist(int noteId)
        {
            return _context.notes.Any(e => e.Id == noteId);
        }
    }
}
