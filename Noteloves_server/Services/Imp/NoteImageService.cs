using Noteloves_server.Data;
using System;
using System.Collections.Generic;
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
    }
}
