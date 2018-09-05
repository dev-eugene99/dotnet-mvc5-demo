using GigHub.Models;
using System;
using System.Linq;

namespace GigHub.ViewModels
{
    public class GigDetailViewModel
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public string Venue { get; set; }

        public DateTime DateTime { get; set; }

        public string Date => DateTime.ToString("yyy MMM d");
        
        public string Time => DateTime.ToString("HH:mm");

        public string Genre { get; set; }

        public ArtistViewModel Artist { get; set; }

        public string Heading { get; set; }

        public bool UserIsFollowingArtist { get; set; }
        public bool UserIsAttending { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse($"{Date} {Time}");

        }

        public GigDetailViewModel(Gig gig, string userId = null)
        {

            Id = gig.Id;
            IsCanceled = gig.IsCanceled;
            DateTime = gig.DateTime;
            Genre = gig.Genre.Name;
            Venue = gig.Venue;
            Artist = new ArtistViewModel
            {
                Id = gig.ArtistId,
                Name = gig.Artist.Name
            };
            UserIsAttending = false;
            UserIsFollowingArtist = false;

            if (!string.IsNullOrEmpty(userId))
            {
                UserIsAttending = gig.Attendances
                    .Any(a => a.AttendeeId == userId);

                UserIsFollowingArtist = gig.Artist.Followers
                    .Any(f => f.FollowerId == userId);
            }
        }
    }
}