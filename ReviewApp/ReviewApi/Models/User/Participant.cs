﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewApi.Models.User
{
    public class Participant
    {
        public int ProjectId { get; set; }
        public List<string> AddedUsers { get; set; }
        public List<string> RemovedUsers { get; set; }
    }
}
