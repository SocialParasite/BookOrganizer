using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetSelectedAsync(Guid id);

        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();

        bool HasChanges();

        void ResetTracking(T entity);
    }
}
