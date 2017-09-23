using System.Collections.Generic;
using GigHub.Controllers;
using GigHub.Core.Models;

namespace GigHub.Core.Repositories
{
    public interface IGigsRepositories
    {
        IEnumerable<Gig> GetGigsUserAttending(string userId);
        Gig GetGigWithAttendees(int gigId);
        IEnumerable<Gig> GetMyGigs(string userId, GigsController gigsController);
        Gig GetGigByIdForCurrentArtist(int id, string userId);
        Gig GetGigWithArtist(int id);
        void Add(Gig gig);
       
    }
}