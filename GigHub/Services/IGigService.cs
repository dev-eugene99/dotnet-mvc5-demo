﻿using GigHub.Models;
using System;
using System.Collections.Generic;

namespace GigHub.Services
{
    public interface IGigService
    {
        Tuple<int, string> AddGig(Gig gig);
        Tuple<int, string> EditGig(Gig gig);
        Tuple<int, string> DeleteGig(Gig gig);

        IEnumerable<Gig> GetUpcomingGigs();
        IEnumerable<Genre> GetGenres();
    }
}