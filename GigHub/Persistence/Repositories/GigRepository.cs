using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Persistence.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Gig> GetUpcomingGigs(string query)
        {

            var upcomingGigs = _context.Gigs
                .Where(g => g.DateTime > DateTime.Now && g.IsCanceled == false)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Include(g => g.Artist.Followers.Select(f => f.Follower));

            if (!string.IsNullOrEmpty(query))
                upcomingGigs = upcomingGigs
                    .Where(g =>
                    g.Artist.Name.Contains(query) ||
                    g.Genre.Name.Contains(query) ||
                    g.Venue.Contains(query));

            return upcomingGigs;
        }


        public async Task<Gig> GetGigAsync(int gigId)
        {
            var gig = await _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefaultAsync(g => g.Id == gigId);
            return gig;
        }

        public async Task<Gig> GetGigDetailByIdAsync(int gigId)
        {
            var gig = await _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Include(g => g.Genre)
                .Include(g => g.Artist)
                .Include(g => g.Artist.Followers.Select(f => f.Follower))
                .SingleAsync(g => g.Id == gigId);
            return gig;
        }



        public async Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId)
        {
            var content = await _context.Gigs
                    .Where(g =>
                            g.ArtistId == artistId &&
                            g.DateTime > DateTime.Now &&
                            g.IsCanceled == false)
                    .Include(g => g.Attendances.Select(a => a.Attendee))
                    .Include(g => g.Artist.Followers.Select(f => f.Follower))
                    .Include(g => g.Genre)
                    .ToListAsync();
            return content;
        }

        public async Task<IList<Gig>> GetGigsByAttendeeIdAsync(string attendeeId)
        {
            var content = await _context.Attendances
                .Where(a => a.AttendeeId == attendeeId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToListAsync();
            return content;
        }

        public void AddGig(Gig gig)
        {
            gig.Artist = _context.Users.Where(u => u.Id == gig.ArtistId)
                .Include(a => a.Followers.Select(f => f.Follower))
                .SingleOrDefault();
            _context.Gigs.Add(gig);
            gig.Create();
        }

        public void CancelGig(Gig gig)
        {
            gig.Cancel();
        }

        public void UpdateGig(Gig gig, DateTime newDate, byte newGenreId, String newVenue)
        {
            gig.Modify(newDate, newGenreId, newVenue);
        }
    }
}