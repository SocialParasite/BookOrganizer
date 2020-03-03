using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task AddNewGenreAsync(Genre genre);
    }
}
