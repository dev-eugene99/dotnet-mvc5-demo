using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        Tuple<int, string> IGigService.AddGig(Gig gig)
        {
            var status = Tuple.Create(0, "SUCCESS");
            try
            {
                _context.Gigs.Add(gig);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                status = Tuple.Create(-1, $"FAILURE: {ex.Message}");
            }

            return status;
        }

        Tuple<int, string> IGigService.DeleteGig(Gig gig)
        {
            throw new NotImplementedException();
        }

        Tuple<int, string> IGigService.EditGig(Gig gig)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Genre> IGigService.GetGenres()
        {
            return _context.Genres;
        }
    }
}