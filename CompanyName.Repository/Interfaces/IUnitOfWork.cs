namespace CompanyName.Repository.Interfaces
{
    /// <summary>
    /// Unit of work contracts.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Get the repository by entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns></returns>
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves the changes in a single transaction.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
