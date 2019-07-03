using BookOrganizer.Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookOrganizer.Data.Repositories
{
    public class BaseRepository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : class
        where TContext : BookOrganizerDbContext
    {
        protected readonly TContext context;

        public BaseRepository(TContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await context.Set<TEntity>().ToListAsync();

        public virtual async Task<TEntity> GetSelectedAsync(Guid id)
            => await context.Set<TEntity>().FindAsync(id);

        public async Task SaveAsync()
            => await context.SaveChangesAsync();

        public void Update(TEntity entity)
            => context.Update(entity);

        public void Delete(TEntity entity)
            => context.Remove(entity);

        public bool HasChanges()
            => context.ChangeTracker.HasChanges();

        public void ResetTracking(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Unchanged;
        }
    }
}
