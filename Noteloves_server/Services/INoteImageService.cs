using Microsoft.AspNetCore.Http;
using Noteloves_server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public interface INoteImageService
    {
        void AddListImage(int noteId, List<IFormFile> images);
        byte[] GetFirstImage(int noteId);
        bool CheckExistImage(int noteId);
        List<NoteImage> GetListImage(int noteId);
    }
}
