using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SMARTBusinessTest.Domain.Exceptions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using SMARTBusinessTest.Application.Constants;
using SMARTBusinessTest.Web.Filters;

namespace SMARTBusinessTest.UnitTests.Filters
{
    [TestFixture]
    public class ContractExceptionFilterTests
    {
        private Mock<ILoggerFactory> _loggerFactoryMock;
        private Mock<ILogger<ContractExceptionFilter>> _loggerMock;
        private ContractExceptionFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ContractExceptionFilter>>();
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerFactoryMock
                .Setup(factory => factory.CreateLogger(It.IsAny<string>()))
                .Returns(_loggerMock.Object);
            _filter = new ContractExceptionFilter(_loggerFactoryMock.Object);
        }

        [Test]
        public void OnException_ShouldHandleOperationCanceledException()
        {
            // Arrange
            var exception = new OperationCanceledException(ExceptionConstants.TokenCancelled);
            var context = CreateExceptionContext(exception);

            // Act
            _filter.OnException(context);

            // Assert
            Assert.IsTrue(context.ExceptionHandled);
            Assert.That(context.HttpContext.Response.StatusCode, Is.EqualTo(400));
            var result = context.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.That(((ContractExceptionResult)result.Value).StatusCode, Is.EqualTo(400));
            Assert.That(((ContractExceptionResult)result.Value).Message, Is.EqualTo(ExceptionConstants.TokenCancelled));
        }

        [Test]
        public void OnException_ShouldHandleInvalidInputDataException()
        {
            // Arrange
            var exception = new InvalidInputDataException(ExceptionConstants.InvalidInputData);
            var context = CreateExceptionContext(exception);

            // Act
            _filter.OnException(context);

            // Assert
            Assert.IsTrue(context.ExceptionHandled);
            Assert.That(context.HttpContext.Response.StatusCode, Is.EqualTo(400));
            var result = context.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.That(((ContractExceptionResult)result.Value).StatusCode, Is.EqualTo(400));
            Assert.That(((ContractExceptionResult)result.Value).Message, Is.EqualTo(ExceptionConstants.InvalidInputData));
        }

        [Test]
        public void OnException_ShouldHandleOutOfPlacementException()
        {
            // Arrange
            var exception = new OutOfPlacementException(ExceptionConstants.OutOfPlacement);
            var context = CreateExceptionContext(exception);

            // Act
            _filter.OnException(context);

            // Assert
            Assert.IsTrue(context.ExceptionHandled);
            Assert.That(context.HttpContext.Response.StatusCode, Is.EqualTo(400));
            var result = context.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.That(((ContractExceptionResult)result.Value).StatusCode, Is.EqualTo(400));
            Assert.That(((ContractExceptionResult)result.Value).Message, Is.EqualTo(ExceptionConstants.OutOfPlacement));
        }

        [Test]
        public void OnException_ShouldHandleOtherExceptions()
        {
            // Arrange
            var exception = new Exception(ExceptionConstants.InternalServerError);
            var context = CreateExceptionContext(exception);

            // Act
            _filter.OnException(context);

            // Assert
            Assert.IsTrue(context.ExceptionHandled);
            Assert.That(context.HttpContext.Response.StatusCode, Is.EqualTo(500));
            var result = context.Result as ObjectResult;
            Assert.IsNotNull(result);
            Assert.That(((ContractExceptionResult)result.Value).StatusCode, Is.EqualTo(500));
            Assert.That(((ContractExceptionResult)result.Value).Message, Is.EqualTo(ExceptionConstants.InternalServerError));
        }

        private ExceptionContext CreateExceptionContext(Exception exception)
        {
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new ActionDescriptor();

            var actionContext = new ActionContext
            {
                HttpContext = httpContext,
                RouteData = routeData,
                ActionDescriptor = actionDescriptor
            };
            return new ExceptionContext(actionContext, new List<IFilterMetadata>())
            {
                Exception = exception
            };
        }
    }
}
