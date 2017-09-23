using System.Collections.Generic;
using GigHub.Core.Models;
using GigHub.Core.Repositories;

namespace GigHub.Persistence.Repositories
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