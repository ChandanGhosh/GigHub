using System.Collections.Generic;
using System.Linq;
using GigHub.Core.Models;

namespace GigHub.Core.ViewModels
{
    public class GigsViewModel
    {
        public GigsViewModel()
        {
        }

        public bool ShowAction { get; set; }
        public IEnumerable<Gig> UpcomingGigs { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public Gig CurrentGig { get; set; }
        public ILookup<int, Attendance> Attendances { get; set; }
    }
}