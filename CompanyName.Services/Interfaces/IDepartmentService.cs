using CompanyName.Model.Models;

namespace CompanyName.Services.Interfaces
{
    /// <summary>
    /// Departments service contracts.
    /// </summary>
    public interface IDepartmentService
    {
        /// <summary>
        /// Get all departments.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A list of departments.</returns>
        Task<IList<DepartmentModel>> GetAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a department by the given department id.
        /// </summary>
        /// <param name="departmentId">The unique id of the department.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A department matching the given department id.</returns>
        Task<DepartmentModel> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken = default);
    }
}