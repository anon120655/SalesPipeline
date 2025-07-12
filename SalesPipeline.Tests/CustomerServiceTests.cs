using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Moq;
using SalesPipeline.Infrastructure.Data.Context;
using SalesPipeline.Infrastructure.Data.Mapping;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Customers;
using Xunit;
using SalesPipeline.Infrastructure.Data.Entity;


namespace SalesPipeline.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task CreateCustomerAsync_ShouldCallRepository()
        {
            // Arrange: สร้าง In-Memory Database
            var options = new DbContextOptionsBuilder<SalesPipelineContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid()) // unique แต่ละ test
                .Options;

            var context = new SalesPipelineContext(options);

            // เตรียมข้อมูลจำลองใน DB (เช่น ตรวจสอบว่ามีอยู่แล้ว)
            context.Customers.Add(new Customer
            {
                JuristicPersonRegNumber = "1234567890123",
                CompanyName = "CompanyName01"
            });
            context.SaveChanges();

            // สร้าง RepositoryWrapper จริง (หรือ mock ถ้าต้องการ)
            var mockRepoBase = new Mock<IRepositoryBase>();

            // จำลอง AppSettings
            var mockOptions = new Mock<IOptions<AppSettings>>();
            mockOptions.Setup(o => o.Value).Returns(new AppSettings());

            // ใช้ AutoMapper Profile จริง
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            var realMapper = config.CreateMapper();

            // สร้าง Mock RepositoryWrapper โดย setup Context ให้คืน SalesPipelineContext จริง
            var mockRepoWrapper = new Mock<IRepositoryWrapper>();
            mockRepoWrapper.Setup(r => r.Context).Returns(context);

            var mockThailand = new Mock<IThailand>();
            mockRepoWrapper.Setup(r => r.Thailand)
               .Returns(mockThailand.Object);

            var mockUserRepo = new Mock<IUserRepo>();
            mockRepoWrapper.Setup(r => r.User)
               .Returns(mockUserRepo.Object);

            // จำลอง Transaction
            var mockTransaction = new Mock<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction>();
            mockRepoWrapper.Setup(r => r.BeginTransaction()).Returns(mockTransaction.Object);

            // สร้าง Service จริง
            var service = new Customers(
                mockRepoWrapper.Object,
                mockRepoBase.Object,
                mockOptions.Object,
                realMapper
            );

            // เตรียม input
            var customerInput = new CustomerCustom
            {
                CurrentUserId = 1,
                JuristicPersonRegNumber = "1234567890124",
                CompanyName = "CompanyName01",
                HouseNo = "10",
                ProvinceId = 1,
                AmphurId = 1,
                TambolId = 1,
            };

            // Act
            var result = await service.Create(customerInput);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("CompanyName01", result.CompanyName);

            // ตรวจว่ามีการเรียก Commit transaction
            mockRepoWrapper.Verify(r => r.BeginTransaction(), Times.Once);
            mockTransaction.Verify(t => t.Commit(), Times.Once);
        }


    }
}
