using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Application.Validators.Tests
{
    public class ValidationBehaviourTests
    {
        private readonly Mock<IValidator<TestRequest>> _validatorMock;
        private readonly ValidationBehaviour<TestRequest, TestResponse> _validationBehaviour;
        private readonly RequestHandlerDelegate<TestResponse> _next;

        public ValidationBehaviourTests()
        {
            _validatorMock = new Mock<IValidator<TestRequest>>();
            var validators = new List<IValidator<TestRequest>> { _validatorMock.Object };
            _validationBehaviour = new ValidationBehaviour<TestRequest, TestResponse>(validators);
            _next = Mock.Of<RequestHandlerDelegate<TestResponse>>();
        }

        [Fact]
        public async Task Should_Call_Next_When_Validation_Passes()
        {
            // Arrange
            var request = new TestRequest();
            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                .Returns(new ValidationResult());

            // Act
            var response = await _validationBehaviour.Handle(request, _next, CancellationToken.None);

            // Assert
            _validatorMock.Verify(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()), Times.Once);
            Mock.Get(_next).Verify(n => n(), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_ValidationException_When_Validation_Fails()
        {
            // Arrange
            var request = new TestRequest();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Property1", "Error message 1"),
                new ValidationFailure("Property2", "Error message 2")
            };
            _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                .Returns(new ValidationResult(failures));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _validationBehaviour.Handle(request, _next, CancellationToken.None));
            Assert.Equal(2, exception.Errors.Count());
            Assert.Contains(exception.Errors, e => e.PropertyName == "Property1" && e.ErrorMessage == "Error message 1");
            Assert.Contains(exception.Errors, e => e.PropertyName == "Property2" && e.ErrorMessage == "Error message 2");
        }

        public class TestRequest : IRequest<TestResponse> { }

        public class TestResponse { }
    }
}
