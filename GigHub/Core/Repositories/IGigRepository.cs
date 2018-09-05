using GigHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Core.Repositories
{
    public interface IGigRepository
    {
        Task<Gig> GetGigAsync(int gigId);
        void AddGig(Gig gig);
        void UpdateGig(Gig gig, DateTime newDate, byte newGenreId, String newVenue);
        void CancelGig(Gig gig);
        
        Task<Gig> GetGigDetailByIdAsync(int gigId);
        IQueryable<Gig> GetUpcomingGigs(string query);
        Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId);
        Task<IList<Gig>> GetGigsByAttendeeIdAsync(string attendeeId);
    }
}