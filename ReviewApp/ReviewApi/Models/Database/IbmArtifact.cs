﻿using System;
using System.Collections.Generic;

namespace ReviewApi.Models.Database
{
    public partial class IbmArtifact
    {
        public int Id { get; set; }
        public int IbmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int WorkproductId { get; set; }
        public int? ReviewId { get; set; }

        public virtual Review Review { get; set; }
        public virtual Workproduct Workproduct { get; set; }
    }
}
