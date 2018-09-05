using GigHub.Interfaces;
using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository()
        {
            _context = new ApplicationDbContext();
        }

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetGenres()
        {
            return _context.Genres;
        }
    }
}