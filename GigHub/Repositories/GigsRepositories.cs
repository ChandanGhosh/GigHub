﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{
    public class GigsRepositories : IGigsRepositories
    {
        private readonly ApplicationDbContext _dbContext;


        public GigsRepositories(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Gig> GetGigsUserAttending(string userId)
        {
            return _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Gig)
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .ToList();
        }

        public Gig GetGigWithAttendees(int gigId)
        {
            return _dbContext.Gigs.Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }

        public IEnumerable<Gig> GetMyGigs(string userId, GigsController gigsController)
        {
            return _dbContext.Gigs.Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && !g.IsCanceled).Include(g => g.Genre);
        }

        public Gig GetGigByIdForCurrentArtist(int id, string userId)
        {
            return _dbContext.Gigs.Single(g => g.Id == id && g.ArtistId == userId);
        }

        public Gig GetGigWithArtist(int id)
        {
            return _dbContext.Gigs.Include(g => g.Artist).SingleOrDefault(g => g.Id == id);
        }

        public void Add(Gig gig)
        {
            _dbContext.Gigs.Add(gig);
        }
    }
}