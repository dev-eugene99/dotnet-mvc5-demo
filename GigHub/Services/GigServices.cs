using GigHub.Models;
using System;

namespace GigHub.Services
{
    public class GigServices : IGigService
    {
        private readonly ApplicationDbContext _context;

        public GigServices()
        {
            _context = new ApplicationDbContext();
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
    }
}