using GigHub.Models;
using System.Collections.Generic;

namespace GigHub.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetGenres();
    }
}
