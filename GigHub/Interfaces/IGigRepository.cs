using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Interfaces
{
    public interface IGigRepository
    {
        Task<Tuple<int, string>> AddGigAsync(Gig gig);
        Task<Tuple<int, string>> UpdateGigAsync(Gig gig, DateTime newDate, byte newGenreId, String newVenue);
        Task<Tuple<int, string>> CancelGigAsync(Gig gig);

        Task<Gig> GetGigAsync(int gigId);
        Task<Gig> GetGigDetailByIdAsync(int gigId);
        IQueryable<Gig> GetUpcomingGigs();
        Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId);
        Task<IList<Gig>> GetGigsByAttendeeIdAsync(string attendeeId);
    }
}