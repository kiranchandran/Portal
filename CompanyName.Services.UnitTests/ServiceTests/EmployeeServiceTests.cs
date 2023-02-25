using AutoMapper;
using CompanyName.Data.Entity;
using CompanyName.Model.Exceptions;
using CompanyName.Model.Models;
using CompanyName.Repository.Interfaces;
using CompanyName.Services.Implementations;
using CompanyName.Services.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Services.UnitTests.ServiceTests
{
    public class EmployeeServiceTests
    {
        private readonly Guid departmentId = Guid.NewGuid();
        public EmployeeServiceTests()
        {
        }

        [Fact]
        public async void VerifyEmployeeSaveIsExecuted()
        {
            var mocks = GetServiceAndUnitOfWork();
            var request = new SaveEmployeeRequest { DateOfBirth = DateTime.UtcNow, Email = "test@test.com", Name = "Test User", DepartmentId = departmentId };
            var result = await mocks.Item1.AddAsync(request);

            mocks.Item2.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task EnsureOnCreateNullRequestThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(default));
        }

        [Fact]
        public async Task EnsureOnCreateEmptyRequestThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(new SaveEmployeeRequest()));
        }

        [Fact]
        public async Task EnsureOnCreateEmptyNameThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(new SaveEmployeeRequest { DateOfBirth = DateTime.UtcNow, Email = "test@test.com", Name = string.Empty, DepartmentId = departmentId }));
        }

        [Fact]
        public async Task EnsureOnCreateEmptyEmailThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(new SaveEmployeeRequest { DateOfBirth = DateTime.UtcNow, Email = string.Empty, Name = "Test User", DepartmentId = departmentId }));
        }

        [Fact]
        public async Task EnsureOnCreateNonExistingDepartmentIdThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(new SaveEmployeeRequest { DateOfBirth = DateTime.UtcNow, Email = "test@test.com", Name = "Test User" }));
        }

        [Fact]
        public async Task EnsureOnCreateInvalidDateThrowsException()
        {
            IEmployeeService service = GetMockedService();
            await Assert.ThrowsAsync<ApiException>(async () => await service.AddAsync(new SaveEmployeeRequest { DateOfBirth = DateTime.MinValue, Email = "test@test.com", Name = "Test User", DepartmentId = departmentId }));
        }       

        private (IEmployeeService, Mock<IUnitOfWork>) GetServiceAndUnitOfWork()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockEmployeeRepo = new Mock<IRepository<Employee>>();
            var mockDepartmentRepo = new Mock<IRepository<Department>>();

            mockUnitOfWork.Setup(s => s.Repository<Employee>()).Returns(mockEmployeeRepo.Object);
            mockUnitOfWork.Setup(s => s.Repository<Department>()).Returns(mockDepartmentRepo.Object);

            mockDepartmentRepo.Setup(s => s.GetAsync(departmentId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTestDepartment()));

            IEmployeeService service = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);
            return (service, mockUnitOfWork);
        }

        private IEmployeeService GetMockedService()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockMapper = new Mock<IMapper>();
            var mockEmployeeRepo = new Mock<IRepository<Employee>>();
            var mockDepartmentRepo = new Mock<IRepository<Department>>();

            mockUnitOfWork.Setup(s => s.Repository<Employee>()).Returns(mockEmployeeRepo.Object);
            mockUnitOfWork.Setup(s => s.Repository<Department>()).Returns(mockDepartmentRepo.Object);

            mockDepartmentRepo.Setup(s => s.GetAsync(departmentId, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(GetTestDepartment()));

            IEmployeeService service = new EmployeeService(mockUnitOfWork.Object, mockMapper.Object);
            return service;
        }
        private Department? GetTestDepartment()
        {
            return new Department { Id = departmentId, Name = "Dummy Department" };
        }
    }
}
