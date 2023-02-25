using CompanyName.Model.Models;
using CompanyName.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyName.Api.Controllers
{
    /// <summary>
    /// Department service endpoint.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Policy = "ApiAcessPolicy")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService departmentService;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="departmentService"><see cref="IDepartmentService"/>.</param>
        public DepartmentsController(IDepartmentService departmentService)
        {
            this.departmentService = departmentService;
        }

        /// <summary>
        /// Get all departments.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns>A list of departments.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<DepartmentModel>))]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var result = await departmentService.GetAsync(cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Returns a department by the given department id.
        /// </summary>
        /// <param name="id">The unique id of the department.</param>
        /// <param name="cancellationToken">Cancellation token <see cref="CancellationToken"/>.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepartmentModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok(await departmentService.GetByIdAsync(id,cancellationToken));
        }

    }
}
