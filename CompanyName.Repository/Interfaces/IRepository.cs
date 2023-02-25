using System.Linq.Expressions;

namespace CompanyName.Repository.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Creates a new entity object.
        /// </summary>
        /// <param name="entity">The new entity object.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>The new entity object.</returns>
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing entity object.
        /// </summary>
        /// <param name="id">Unique id of the entity object</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if the object exists based on the give condition.
        /// </summary>
        /// <param name="predicate">The predicate with conditions.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>true of false</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the first entity which is satisfying the predicate condition.
        /// </summary>
        /// <param name="predicate">The predicate with conditions.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all the entity objects
        /// </summary>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>Entity objects</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the entity object by id.
        /// </summary>
        /// <param name="id">The unique id of the entity.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the IQueryable.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();

        /// <summary>
        /// Returns the IQueryable based on condition.
        /// </summary>
        /// <param name="predicate">The predicate with conditions.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        IQueryable<T> Query(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a single object or null satisfying the condition. If there are more than one item satisfying the condition then it throws an exception.
        /// </summary>
        /// <param name="predicate">The predicate with conditions.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        void Update(T entity, CancellationToken cancellationToken = default);
    }
}
