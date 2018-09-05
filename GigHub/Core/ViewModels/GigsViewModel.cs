﻿using System.Collections.Generic;

namespace GigHub.Core.ViewModels
{
    public class GigsViewModel
    {
        public IEnumerable<GigDetailViewModel> Gigs { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
    }
}