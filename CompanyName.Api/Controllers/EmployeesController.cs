using CompanyName.Model.Models;
using CompanyName.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.Api.Controllers
{
    /// <summary>
    /// Employee service endpoint.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Policy = "ApiAcessPolicy")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="employeeService"><see cref="IEmployeeService"/>.</param>
        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        /// <summary>
        /// Search the user by the given input parameters.
        /// </summary>
        /// <param name="model">The search input parameters.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A list of employees matches the search criteria.</returns>
        [HttpPost("Search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<EmployeeModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromBody] EmployeeSearchRequest model, CancellationToken cancellationToken = default)
        {
            var result = await employeeService.GetAsync(model, cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Creates a new employee with the provider details.
        /// </summary>
        /// <param name="model">The new employee details.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A new employee.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SaveEmployeeRequest model, CancellationToken cancellationToken = default)
        {
            var result = await employeeService.AddAsync(model, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Get the employee by the given employee id.
        /// </summary>
        /// <param name="id">The unique id of the employee.</param> 
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>An employee matching the given employee id.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok(await employeeService.GetByIdAsync(id, cancellationToken));
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The unique id of the employee.</param>
        /// <param name="model">The employee details to update.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>Updated employee details.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SaveEmployeeRequest model, CancellationToken cancellationToken = default)
        {
            var result = await employeeService.UpdateAsync(id, model, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing employee. This is a soft delete operation.
        /// </summary>
        /// <param name="id">The unique id of the employee.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>true of false indicating the success of the delete operation.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(void))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await employeeService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
