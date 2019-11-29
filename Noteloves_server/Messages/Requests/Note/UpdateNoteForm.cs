using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Requests.Note
{
    public class UpdateNoteForm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Anniversary { get; set; }
        public bool Hidden { get; set; }
    }
}
