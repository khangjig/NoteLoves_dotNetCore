using Microsoft.AspNetCore.Http;
using Noteloves_server.Data;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class NoteImageService : INoteImageService
    {
        private readonly DatabaseContext _context;

        public NoteImageService(DatabaseContext context)
        {
            _context = context;
        }

        private void AddImage(int noteId, IFormFile image)
        {
            NoteImage noteImage = new NoteImage();

            noteImage.NoteId = noteId;
            noteImage.Image = EncodeImage(image);

            _context.note_images.Add(noteImage);
        }

        public void AddListImage(int noteId, List<IFormFile> images)
        {
            foreach (IFormFile image in images)
            {
                AddImage(noteId, image);
            }
        }

        public byte[] GetFirstImage(int noteId)
        {
            var image = _context.note_images.First(x => x.NoteId == noteId);
            return image.Image;
        }

        public List<NoteImage> GetListImage(int noteId)
        {
            return _context.note_images.Where(x => x.NoteId == noteId).ToList();
        }

        public bool CheckExistImage(int noteId)
        {
            return _context.note_images.Any(x => x.NoteId == noteId);
        }

        private byte[] EncodeImage(IFormFile image)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }
    }
}
