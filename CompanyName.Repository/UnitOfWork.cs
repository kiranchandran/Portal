using CompanyName.Data.Contexts;
using CompanyName.Repository.Interfaces;

namespace CompanyName.Repository
{
    /// <inheritdoc />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext context;
        private IDictionary<Type, object> repositories;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="context">Data context <see cref="DataContext"/>.</param>
        public UnitOfWork(DataContext context)
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            this.context = context;
        }

        /// <inheritdoc />
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (context.Set<TEntity>() == null)
            {
                throw new NotSupportedException($"{typeof(TEntity)} not a valid DbSet");
            }

            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
