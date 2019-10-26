using Noteloves_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Services.Imp
{
    public class NoteService : INoteService
    {
        private readonly DatabaseContext _context;

        public NoteService(DatabaseContext context)
        {
            _context = context;
        }
    }
}
