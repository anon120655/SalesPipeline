using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Moq;
using SalesPipeline.Infrastructure.Data.Entity;
using SalesPipeline.Infrastructure.Data.Mapping;
using SalesPipeline.Infrastructure.Interfaces;
using SalesPipeline.Infrastructure.Repositorys;
using SalesPipeline.Infrastructure.Wrapper;
using SalesPipeline.Utils;
using SalesPipeline.Utils.Resources.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SalesPipeline.Tests
{
    public class MasterDataServiceTests
    {
        [Fact] // บอกว่าเมธอดนี้คือ Unit Test แบบไม่มี input (จาก xUnit)
        public async Task Create_ShouldReturnMappedMasterChainCustom()
        {
            // Arrange: เตรียม Mock ของ dependencies ที่ MasterChains ต้องใช้
            var mockRepo = new Mock<IRepositoryWrapper>();               // Mock สำหรับ Repository Wrapper ที่ใช้เรียก BeginTransaction
            var mockDb = new Mock<IRepositoryBase>();                    // Mock สำหรับฐานข้อมูลที่ใช้ InsterAsync, SaveAsync
            var mockMapper = new Mock<IMapper>();                        // Mock AutoMapper สำหรับแปลง Entity เป็น DTO
            var mockOptions = new Mock<IOptions<AppSettings>>();         // Mock ค่า config (AppSettings)
            var mockTransaction = new Mock<IDbContextTransaction>();     // Mock Transaction ของ EF Core

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
            var realMapper = config.CreateMapper();

            // Setup: เมื่อ BeginTransaction() ถูกเรียก ให้คืน mockTransaction ที่เตรียมไว้
            mockRepo.Setup(r => r.BeginTransaction())
                    .Returns(mockTransaction.Object);

            // Setup: Mock AppSettings ให้พร้อมใช้งานใน constructor
            //mockOptions.Setup(o => o.Value).Returns(new AppSettings());

            // เตรียม input ที่จะใช้ในการทดสอบ
            var input = new Master_ChainCustom
            {
                Name = "ผู้ประกอบการค้า",
                CurrentUserId = 0
            };

            // Setup: เมื่อ Map<>() ถูกเรียก ให้คืน DTO ตามชื่อที่ป้อนเข้าไป
            //mockMapper.Setup(m => m.Map<Master_ChainCustom>(It.IsAny<Master_Chain>()))
            //          .Returns((Master_Chain src) => new Master_ChainCustom { Name = src.Name });

            // สร้าง instance ของ service ที่จะทดสอบ พร้อม inject dependency
            var service = new MasterChains(
                mockRepo.Object,
                mockDb.Object,
                mockOptions.Object,
                realMapper
            );

            // Act: เรียก method ที่ต้องการทดสอบ
            var result = await service.Create(input);

            // Assert: ตรวจสอบผลลัพธ์ว่าตรงกับที่คาดหรือไม่
            Assert.NotNull(result); // ตรวจว่าไม่ได้ return null
            Assert.Equal("ผู้ประกอบการค้า", result.Name); // ตรวจว่า Name ถูกต้อง

            // Assert: ตรวจสอบว่าฟังก์ชันภายในถูกเรียกจริงหรือไม่
            mockDb.Verify(d => d.InsterAsync(It.IsAny<Master_Chain>()), Times.Once); // ตรวจว่า InsterAsync ถูกเรียก 1 ครั้ง
            mockDb.Verify(d => d.SaveAsync(), Times.Once);                           // ตรวจว่า SaveAsync ถูกเรียก 1 ครั้ง
            //mockMapper.Verify(m => m.Map<Master_ChainCustom>(It.IsAny<Master_Chain>()), Times.Once); // ตรวจว่า Map ถูกเรียก 1 ครั้ง
            mockTransaction.Verify(t => t.Commit(), Times.Once);                     // ตรวจว่า Commit() ถูกเรียก 1 ครั้ง
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(5, 5, 10)]
        [InlineData(-1, -1, -2)]
        public void Add_ShouldReturnCorrectResult(int a, int b, int expected)
        {
            var result = a + b;
            Assert.Equal(expected, result);
        }

    }
}
