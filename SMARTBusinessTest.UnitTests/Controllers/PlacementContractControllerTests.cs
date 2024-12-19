using Moq;
using Microsoft.AspNetCore.Mvc;
using SMARTBusinessTest.Web.Controllers;
using SMARTBusinessTest.Application.Interfaces;
using SMARTBusinessTest.Application.Commands;
using SMARTBusinessTest.Application.DTOs;

namespace SMARTBusinessTest.UnitTests.Filters
{
    [TestFixture]
    public class PlacementContractControllerTests
    {
        private PlacementContractController _controller;
        private Mock<IPlacementContractService> _mockService;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IPlacementContractService>();
            _controller = new PlacementContractController(_mockService.Object);
        }

        [Test]
        public async Task Get_ShouldReturnOkResultWithContracts()
        {
            // Arrange
            var contracts = new List<PlacementContractDTO> { new PlacementContractDTO { ContractId = Guid.NewGuid() } };
            _mockService.Setup(service => service.GetPlacementContractsAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(contracts);

            // Act
            var result = await _controller.Get(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = (OkObjectResult)result;
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));
            Assert.That(okObjectResult.Value, Is.EqualTo(contracts));
        }

        [Test]
        public async Task Post_WhenModelIsValid_ThenShouldReturnCreatedStatus()
        {
            // Arrange
            var newContract = new PlacementContractCreateCommand();
            _mockService.Setup(service => service.CreatePlacementContractAsync(newContract, It.IsAny<CancellationToken>()))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(newContract, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(201));
            _mockService.Verify(service => service.CreatePlacementContractAsync(newContract, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Post_WhenModelIsInvalid_ThenShouldReturnBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Code", "Required");
            var newContract = new PlacementContractCreateCommand();

            // Act
            var result = await _controller.Post(newContract, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StatusCodeResult>(result);
            var statusCodeResult = (StatusCodeResult)result;
            Assert.That(statusCodeResult.StatusCode, Is.EqualTo(400));
            _mockService.Verify(service => service.CreatePlacementContractAsync(It.IsAny<PlacementContractCreateCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}