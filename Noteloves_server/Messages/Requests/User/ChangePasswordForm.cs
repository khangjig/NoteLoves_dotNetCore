﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Noteloves_server.Messages.Requests.User
{
    public class ChangePasswordForm
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
