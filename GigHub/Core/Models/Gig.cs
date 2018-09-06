using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public DateTime DateTime { get; private set; }
        
        public ApplicationUser Artist { get; set; }

        public string ArtistId { get; set; }

        public string Venue { get; private set; }

        public Genre Genre { get; private set; }

        public byte GenreId { get; private set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig(string artistId, DateTime dateTime, byte genreId, string venue)
        {
            ArtistId = artistId;
            DateTime = dateTime;
            GenreId = genreId;
            Venue = venue;
            Attendances = new Collection<Attendance>();
        }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Create()
        {
            NotifyFollowers(Notification.GigCreated(this));
        }

        public void Modify(DateTime newDate, byte newGenreId, String newVenue)
        {
            var OriginalDateTime = DateTime;
            var OriginalVenue = Venue;

            DateTime = newDate;
            GenreId = newGenreId;
            Venue = newVenue;

            var notification = Notification.GigUpdated(this, OriginalDateTime, OriginalVenue);
            NotifyAttendees(notification);
        }

        public void Cancel()
        {
            IsCanceled = true;
            NotifyAttendees(Notification.GigCanceled(this));
        }

        private void NotifyAttendees(Notification notification)
        {            
            foreach (var attendee in Attendances.Select(a => a.Attendee))
            {
                attendee.Notify(notification);
            }
        }

        private void NotifyFollowers(Notification notification)
        {
            foreach (var follower in Artist.Followers.Select(u => u.Follower))
            {
                follower.Notify(notification);
            }
        }
    }

}
