using CompanyName.Data.Contexts;
using CompanyName.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CompanyName.Repository
{
    /// <inheritdoc />
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> dbSet;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">Data context <see cref="DataContext"/>.</param>
        public Repository(DataContext context)
        {
            this.dbSet = context.Set<T>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await dbSet.AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await dbSet.FindAsync(id, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await Query().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken= default)
        {
            return await Query().SingleOrDefaultAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public IQueryable<T> Query()
        {
            return dbSet.AsQueryable();
        }

        /// <inheritdoc />
        public IQueryable<T> Query(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Query().Where(predicate);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await dbSet.AnyAsync(predicate, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public void Update(T entity, CancellationToken cancellationToken = default)
        {
            dbSet.Update(entity);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T? entity = await dbSet.FindAsync(id, cancellationToken);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }
    }
}