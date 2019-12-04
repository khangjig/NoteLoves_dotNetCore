using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Responses.Note
{
    public class NoteDataResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Anniversary { get; set; }
        public bool Hidden{ get; set; }
        public int UserId { get; set; }
        public string FirstImage { get; set; }

        public NoteDataResponse(int id, string title, string content, DateTime anniversary, bool hidden, int userId)
        {
            Id = id;
            Title = title;
            Content = content;
            Anniversary = anniversary;
            Hidden = hidden;
            UserId = userId;
        }

        public NoteDataResponse(int id, string title, string content, DateTime anniversary, bool hidden, int userId, string firstImage)
        {
            Id = id;
            Title = title;
            Content = content;
            Anniversary = anniversary;
            Hidden = hidden;
            UserId = userId;
            FirstImage = firstImage;
        }
    }
}
