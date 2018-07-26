using GigHub.Interfaces;
using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GigHub.Services
{
    public class GigServices : IGigService
    {
        private readonly ApplicationDbContext _context;

        public GigServices()
        {
            _context = new ApplicationDbContext();
        }


        public IEnumerable<Gig> GetUpcomingGigs()
        {
            return _context.Gigs.Where(g => g.DateTime > DateTime.Now);
        }

        public async Task<Tuple<int, string>> AddGigAsync(Gig gig)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                _context.Gigs.Add(gig);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }

            return status;
        }

        public async Task<Tuple<int, string>> DeleteGigAsync(Gig gig)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<int, string>> UpdateGigAsync(Gig gig)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }
            return status;
        }

        public async Task<Gig> GetGigByIdAsync(int gigId)
        {
            var gig = await _context.Gigs.SingleAsync(g => g.Id == gigId);
            return gig;
        }

        IEnumerable<Genre> IGigService.GetGenres()
        {
            return _context.Genres;
        }

        public async Task<IEnumerable<Gig>> GetUpcomingGigsByArtistIdAsync(string artistId)
        {
            var content = await _context.Gigs
                    .Where(g => g.ArtistId == artistId && g.DateTime > DateTime.Now)
                    .Include(g => g.Genre)
                    .ToListAsync();
            return content;
        }

        public async Task<IEnumerable<Gig>> GetGigsByAttendeeIdAsync(string attendeeId)
        {
            var content = await _context.Attendances
                .Where(a => a.AttendeeId == attendeeId)
                .Select(a => a.Gig)
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .ToListAsync();
            return content;
        }
    }
}