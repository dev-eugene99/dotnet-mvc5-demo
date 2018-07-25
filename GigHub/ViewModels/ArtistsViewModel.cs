using System.Collections.Generic;

namespace GigHub.ViewModels
{
    public class ArtistsViewModel
    {
        public IEnumerable<ArtistViewModel> Artists { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
    }
}