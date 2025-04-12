using API.Middleware;
using Application.DTOs;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Text.Json;

namespace API.UnitTests.Middleware
{
    public class ValidationExceptionMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly ValidationExceptionMiddleware _middleware;

        public ValidationExceptionMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new ValidationExceptionMiddleware(_nextMock.Object);
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_ValidationException_Is_Thrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Property1", "Error message 1"),
                new ValidationFailure("Property2", "Error message 2")
            };
            var validationException = new ValidationException(validationFailures);
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Throws(validationException);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);

            context.Response.Body.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var response = JsonSerializer.Deserialize<ValidationErrorResponseDTO>(responseBody);

            Assert.NotNull(response);
            Assert.Equal(2, response.Errors.Count);
            Assert.Contains(response.Errors, e => e.PropertyName == "Property1" && e.ErrorMessage == "Error message 1");
            Assert.Contains(response.Errors, e => e.PropertyName == "Property2" && e.ErrorMessage == "Error message 2");
        }

        [Fact]
        public async Task Should_Call_Next_When_No_Exception_Is_Thrown()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _nextMock.Setup(next => next(It.IsAny<HttpContext>())).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once);
        }
    }
}