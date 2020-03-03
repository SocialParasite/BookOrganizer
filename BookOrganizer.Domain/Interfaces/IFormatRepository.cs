using System.Threading.Tasks;

namespace BookOrganizer.Domain
{
    public interface IFormatRepository : IRepository<Format>
    {
        Task AddNewFormatAsync(Format format);
    }
}
