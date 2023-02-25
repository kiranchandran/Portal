using AutoMapper;
using CompanyName.Data.Entity;
using CompanyName.Repository.Interfaces;
using CompanyName.Services.Implementations;
using CompanyName.Services.Interfaces;
using Moq;

namespace CompanyName.Services.UnitTests.ServiceTests
{
    public class DepartmentServiceTests
    {
        public DepartmentServiceTests()
        {
        }

        [Fact]
        public async void VerifyDepartmentRetrunsAsExpected()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockRepo = new Mock<IRepository<Department>>();

            mockRepo.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(GetTestDepartments()));

            mockUnitOfWork.Setup(s => s.Repository<Department>()).Returns(mockRepo.Object);
           

            IDepartmentService service = new DepartmentService(mockUnitOfWork.Object, mockMapper.Object);

            var result = await service.GetAsync();

            Assert.NotNull(result);
            Assert.Equal(GetTestDepartments().ToList().Count, result.Count());
        }

        private IEnumerable<Department> GetTestDepartments()
        {
            return new List<Department>
            {
                new Department { Id = Guid.NewGuid(), Name = "Department 1" },
                new Department { Id = Guid.NewGuid(), Name = "Department 2" },
                new Department { Id = Guid.NewGuid(), Name = "Department 3" },
            };
        }
    }
}
