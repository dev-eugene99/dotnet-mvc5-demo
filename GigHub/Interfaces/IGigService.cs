using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IGigService
    {
        Task<Tuple<int, string>> AddGigAsync(Gig gig);
        Task<Tuple<int, string>> UpdateGigAsync(Gig gig);
        Task<Tuple<int, string>> CancelGigAsync(Gig gig);

        Task<Gig> GetGigByIdAsync(int gigId);
        IEnumerable<Gig> GetUpcomingGigs();
        IEnumerable<Genre> GetGenres();
        Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId);
        Task<IEnumerable<Gig>> GetGigsByAttendeeIdAsync(string attendeeId);
    }
}