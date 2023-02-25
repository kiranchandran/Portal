using AutoMapper;
using CompanyName.Data.Entity;
using CompanyName.Model.Exceptions;
using CompanyName.Model.Models;
using CompanyName.Repository.Interfaces;
using CompanyName.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Services.Implementations
{
    /// <inheritdoc />
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">Auto mapper <see cref="IMapper"/>.</param>
        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IList<DepartmentModel>> GetAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<Department> result = await unitOfWork.Repository<Department>().GetAllAsync(cancellationToken);

            return result.Select(mapper.Map<DepartmentModel>).ToList();
        }

        /// <inheritdoc />
        public async Task<DepartmentModel> GetByIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
        {
            var department = await unitOfWork.Repository<Department>().GetAsync(departmentId, cancellationToken);
            if (department is null)
            {
                throw new NotFoundException($"Department with id:{departmentId} not found.");
            }
            return mapper.Map<DepartmentModel>(department);
        }
    }
}
