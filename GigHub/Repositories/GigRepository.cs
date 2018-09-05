using GigHub.Interfaces;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Repositories
{
    public class GigRepository : IGigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository()
        {
            _context = new ApplicationDbContext();
        }

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public IQueryable<Gig> GetUpcomingGigs()
        {
            return _context.Gigs
                .Where(g => g.DateTime > DateTime.Now && g.IsCanceled == false)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Include(g => g.Artist.Followers.Select(f => f.Follower));                
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

        public async Task<Tuple<int, string>> AddGigAsync(Gig gig)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                gig.Artist = await _context.Users.Where(u => u.Id == gig.ArtistId)
                    .Include(a => a.Followers.Select(f => f.Follower))
                    .SingleOrDefaultAsync();
                _context.Gigs.Add(gig);
                gig.Create();

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }

            return status;
        }

        public async Task<Tuple<int, string>> CancelGigAsync(Gig gig)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                gig.Cancel();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }

            return status;
        }

        public async Task<Tuple<int, string>> UpdateGigAsync(Gig gig, DateTime newDate, byte newGenreId, String newVenue)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                gig.Modify(newDate, newGenreId, newVenue);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }
            return status;
        }

    }
}