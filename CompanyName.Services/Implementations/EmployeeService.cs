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
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        // <summary>
        /// Constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit of work <see cref="IUnitOfWork"/>.</param>
        /// <param name="mapper">Auto mapper <see cref="IMapper"/>.</param>
        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<EmployeeModel> AddAsync(SaveEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            ApiException exception = new("Validation Errors");
            if (request == null)
            {
                exception.AddError(nameof(request), $"{nameof(request)} cannot be null or empty.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    exception.AddError(nameof(request.Name), $"{nameof(request.Name)} is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    exception.AddError(nameof(request.Email), $"{nameof(request.Email)} is required.");
                }

                if (request.DateOfBirth.Year < 1900)
                {
                    exception.AddError(nameof(request.DateOfBirth), $"{nameof(request.DateOfBirth)} should be greater than 1900.");
                }

                var department = await unitOfWork.Repository<Department>().GetAsync(request.DepartmentId, cancellationToken);
                if (department is null)
                {
                    exception.AddError(nameof(request.DepartmentId), $"Department with id:{request.DepartmentId} not found.");
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var existingEmployee = await unitOfWork.Repository<Employee>().FirstOrDefaultAsync(s => s.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase), cancellationToken);
                    if(existingEmployee != null)
                    {
                        exception.AddError(nameof(request.Equals), $"Employee with same email {request.Email} already exists.");
                    }
                }
            }

            if (exception.HasErrors)
            {
                throw exception;
            }

            var employee = await unitOfWork.Repository<Employee>().CreateAsync(mapper.Map<Employee>(request), cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<EmployeeModel>(employee);
        }


        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            var existingEmployee = await unitOfWork.Repository<Employee>().Query().FirstOrDefaultAsync(s => s.Id.Equals(employeeId));
            if (existingEmployee == null)
            {
                throw new NotFoundException($"Employee with id:{employeeId} not found.");
            }

            existingEmployee.IsDeleted = true;
            unitOfWork.Repository<Employee>().Update(existingEmployee, cancellationToken);

            return await unitOfWork.SaveChangesAsync(cancellationToken) > 0;
        }

        /// <inheritdoc />
        public async Task<IList<EmployeeModel>> GetAsync(EmployeeSearchRequest? request, CancellationToken cancellationToken = default)
        {
            var query = unitOfWork.Repository<Employee>()
                .Query();
            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    query = query.Where(s => s.Name.Contains(request.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    query = query.Where(s => s.Email.Contains(request.Email, StringComparison.OrdinalIgnoreCase));
                }

                if (request.DepartmentId.HasValue && !request.DepartmentId.Value.Equals(Guid.Empty))
                {
                    query = query.Where(s => s.DepartmentId.Equals(request.DepartmentId));
                }
            }

            IEnumerable<Employee> result = await query.OrderByDescending(s => s.CreatedDate)
                .ToListAsync(cancellationToken);

            return result.Select(mapper.Map<EmployeeModel>).ToList();
        }

        /// <inheritdoc />
        public async Task<EmployeeModel> GetByIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
        {
            var employee = await unitOfWork.Repository<Employee>().GetAsync(employeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException($"Employee with id:{employeeId} not found.");
            }
            return mapper.Map<EmployeeModel>(employee);
        }

        /// <inheritdoc />
        public async Task<EmployeeModel> UpdateAsync(Guid employeeId, SaveEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            ApiException exception = new("Validation Errors");
            if (request == null)
            {
                exception.AddError(nameof(request), $"{nameof(request)} cannot be null or empty.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    exception.AddError(nameof(request.Name), $"{nameof(request.Name)} is required.");
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    exception.AddError(nameof(request.Email), $"{nameof(request.Email)} is required.");
                }

                if (request.DateOfBirth.Year < 1900)
                {
                    exception.AddError(nameof(request.DateOfBirth), $"{nameof(request.DateOfBirth)} should be greater than 1900.");
                }

                var department = await unitOfWork.Repository<Department>().GetAsync(request.DepartmentId, cancellationToken);
                if (department is null)
                {
                    exception.AddError(nameof(request.DepartmentId), $"Department with id:{request.DepartmentId} not found.");
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var existingEmployee = await unitOfWork.Repository<Employee>().FirstOrDefaultAsync(s => s.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase) && !s.Id.Equals(employeeId), cancellationToken);
                    if (existingEmployee != null)
                    {
                        exception.AddError(nameof(request.Equals), $"Employee with same email {request.Email} already exists.");
                    }
                }
            }

            if (exception.HasErrors)
            {
                throw exception;
            }

            var employee = await unitOfWork.Repository<Employee>().GetAsync(employeeId, cancellationToken);
            if (employee is null)
            {
                throw new NotFoundException($"Employee with id:{employeeId} not found.");
            }

            employee = mapper.Map(request, employee);
            unitOfWork.Repository<Employee>().Update(employee, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return mapper.Map<EmployeeModel>(employee);
        }
    }
}
