using Noteloves_server.Messages.Requests.Note;
using Noteloves_server.Messages.Responses.Note;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services
{
    public interface INoteService
    {
        void AddNote(int userId, AddNoteForm addNoteForm);
        void UpdateNote(UpdateNoteForm updateNoteForm);
        void DeleteNote(int noteId);
        Note GetNoteById(int noteId);
        List<NoteDataResponse> GetListNote(int userId, int page, int size);
        List<NoteDataResponse> GetNoteOnThisDay(int userID);
        List<NoteDataResponse> GetListNoteByWeek(int userID);
        List<NoteDataResponse> GetListNoteInMonth(int userID);
        int GetNewestNote(int userID);
        int GetUserIDByNoteID(int noteID);
        int GetPartnerIDByNoteID(int noteID);
        bool CheckNoteByUser(int userId, int noteId);
        bool CheckNoteByPartner(int partnerId, int noteId);
        bool CheckNoteExist(int noteId);
    }
}
