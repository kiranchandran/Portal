using CompanyName.Model.Models;

namespace CompanyName.Services.Interfaces
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Search the user by the given input parameters.
        /// </summary>
        /// <param name="request">The search input parameters.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A list of employees matches the search criteria.</returns>
        Task<IList<EmployeeModel>> GetAsync(EmployeeSearchRequest? request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the employee by the given employee id.
        /// </summary>
        /// <param name="employeeId">The unique id of the employee.</param> 
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>An employee matching the given employee id.</returns>
        Task<EmployeeModel> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a new employee with the provider details.
        /// </summary>
        /// <param name="request">The new employee details.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A new employee.</returns>
        Task<EmployeeModel> AddAsync(SaveEmployeeRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="employeeId">The unique id of the employee.</param>
        /// <param name="request">The employee details to update.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>Updated employee details.</returns>
        Task<EmployeeModel> UpdateAsync(Guid employeeId, SaveEmployeeRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an existing employee. This is a soft delete operation.
        /// </summary>
        /// <param name="employeeId">The unique id of the employee.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>true of false indicating the success of the delete operation.</returns>
        Task<bool> DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default);
    }
}
