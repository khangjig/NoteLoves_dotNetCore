using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Responses
{
    public class DataResponse
    {
        public string Status { get; set; }
        public object Results { get; set; }
        public string Messages { get; set; }

        public DataResponse(string Status, object Results, string Messages)
        {
            this.Status = Status;
            this.Results = Results;
            this.Messages = Messages;
        }
    }
}
