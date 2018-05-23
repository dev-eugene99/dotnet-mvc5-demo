using GigHub.Models;
using System;

namespace GigHub.Services
{
    public interface IGigService
    {
        Tuple<int, string> AddGig(Gig gig);
        Tuple<int, string> EditGig(Gig gig);
        Tuple<int, string> DeleteGig(Gig gig);
    }
}