using Microsoft.AspNetCore.Mvc;
using Moq;
using JustShop2.Controllers;
using JustShop2.Core.Domain;
using JustShop2.Core.Dto;
using JustShop2.Core.ServiceInterface;
using JustShop2.Models.Kindergartens;

namespace JustShop2.Tests
{
    public class KindergartenTests : TestBase
    {
        private readonly Mock<IKindergartenServices> _mockKindergartenService;
        private readonly Mock<IFileServices> _mockFileService;
        private readonly KindergartensController _controller;

        public KindergartenTests()
        {
            _mockKindergartenService = new Mock<IKindergartenServices>();
            _mockFileService = new Mock<IFileServices>();

            _controller = new KindergartensController(_context, _mockKindergartenService.Object, _mockFileService.Object);
        }

        [Fact]
        public async Task ShouldCreateKindergarten()
        {
            // Arrange
            var model = new KindergartenCreateUpdateViewModel
            {
                KindergartenName = "Test Kindergarten",
                GroupName = "Kotkas",
                Teacher = "Vanaisa",
                ChildrenCount = 5
            };

            var dto = new KindergartenDto
            {
                KindergartenName = model.KindergartenName,
                GroupName = model.GroupName,
                Teacher = model.Teacher,
                ChildrenCount = model.ChildrenCount,
            };

            var createdKindergarten = new Kindergarten
            {
                Id = Guid.NewGuid(),
                KindergartenName = dto.KindergartenName,
                GroupName = dto.GroupName,
                Teacher = dto.Teacher,
                ChildrenCount = dto.ChildrenCount ?? 0
            };

            _mockKindergartenService
                .Setup(s => s.Create(It.IsAny<KindergartenDto>()))
                .ReturnsAsync(createdKindergarten);

            // Act
            var result = await _controller.Create(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }


        // Test 2: Lasteaia pärimine ID alusel
        [Fact]
        public async Task ShouldGetKindergartenById()
        {
            // Arrange
            var id = Guid.NewGuid();
            var kindergarten = new Kindergarten
            {
                Id = id,
                KindergartenName = "Test Kindergarten",
                GroupName = "Öökull",
                Teacher = "Rebane",
                ChildrenCount = 10,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _mockKindergartenService
                .Setup(s => s.DetailAsync(id))
                .ReturnsAsync(kindergarten);

            // Act
            var result = await _controller.Details(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<KindergartenDetailsViewModel>(viewResult.Model);
            Assert.Equal(id, model.Id);
        }


        // Test 3: Lasteaia uuendamine
        [Fact]
        public async Task ShouldUpdateKindergarten()
        {
            // Arrange
            var id = Guid.NewGuid();
            var viewModel = new KindergartenCreateUpdateViewModel
            {
                Id = id,
                KindergartenName = "Updated Kindergarten",
                GroupName = "Pääsuke",
                Teacher = "Ema",
                ChildrenCount = 1
            };

            var dto = new KindergartenDto
            {
                Id = viewModel.Id.Value,
                KindergartenName = viewModel.KindergartenName,
                GroupName = viewModel.GroupName,
                Teacher = viewModel.Teacher,
                ChildrenCount = viewModel.ChildrenCount,
            };

            var updatedKindergarten = new Kindergarten
            {
                Id = dto.Id,
                KindergartenName = dto.KindergartenName,
                GroupName = dto.GroupName,
                Teacher = dto.Teacher,
                ChildrenCount = dto.ChildrenCount ?? 0,
            };

            _mockKindergartenService
                .Setup(s => s.Update(It.IsAny<KindergartenDto>()))
                .ReturnsAsync(updatedKindergarten);

            // Act
            var result = await _controller.Update(viewModel);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }





        // Test 4: Lasteaia kustutamine
        [Fact]
        public async Task ShouldDeleteKindergarten()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deletedKindergarten = new Kindergarten
            {
                Id = id,
                KindergartenName = "Test Kindergarten",
                GroupName = "Sipelgapesa",
                Teacher = "Aare",
                ChildrenCount = 20,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _mockKindergartenService
                .Setup(s => s.Delete(id))
                .ReturnsAsync(deletedKindergarten);

            // Act
            var result = await _controller.DeleteConfirmation(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

    }
}