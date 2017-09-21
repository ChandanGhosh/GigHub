using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GigsDetailsViewModel
    {
        public GigsDetailsViewModel()
        {
        }

        public Gig Gig { get; set; }
        public bool isAttending { get; set; }
        public bool isFollowing { get; set; }
    }
}