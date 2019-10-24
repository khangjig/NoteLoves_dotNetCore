using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Responses
{
    public class ErrorResponse
    {
        public string Status { get; set; }
        public string Messages { get; set; }

        public ErrorResponse(string Status, string Messages)
        {
            this.Status = Status;
            this.Messages = Messages;
        }
    }
}
