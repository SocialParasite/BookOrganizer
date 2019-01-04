using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookOrganizer.UI.WPF.Repositories
{
    public class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        public BaseRepository(TContext context)
        {
            this.context = context;
        }

        private readonly TContext context;

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetSelectedAsync(Guid id)
            => await context.Set<TEntity>().FindAsync(id);

        //public void Insert(TEntity entity) => context.Set<TEntity>().Add(entity);
        public async Task SaveAsync() => await context.SaveChangesAsync();
        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);

    }
}
