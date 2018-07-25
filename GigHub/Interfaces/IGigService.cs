using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IGigService
    {
        Tuple<int, string> AddGig(Gig gig);
        Tuple<int, string> EditGig(Gig gig);
        Tuple<int, string> DeleteGig(Gig gig);

        IEnumerable<Gig> GetUpcomingGigs();        
        IEnumerable<Genre> GetGenres();
        Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId);
        Task<IEnumerable<Gig>> GetGigsByAttendeeIdAsync(string attendeeId);
    }
}