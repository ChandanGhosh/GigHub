using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Persistence;

namespace GigHub.Repositories
{


    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _dbContext.Genres;
        }
    }
}