using Application.Features.Commands.DeleteBook;
using Domain.Interfaces;
using MediatR;
using Moq;

namespace BookManagementUnitTests.HandlerTests
{
    public class DeleteBookCommandHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly DeleteBookCommandHandler _handler;

        public DeleteBookCommandHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _handler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_DeletesBookSuccessfully()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            _bookRepositoryMock.Setup(r => r.DeleteBookAsync(command.BookId)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookRepositoryMock.Verify(r => r.DeleteBookAsync(command.BookId), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_OnRepositoryError()
        {
            // Arrange
            var command = new DeleteBookCommand(Guid.NewGuid());

            _bookRepositoryMock.Setup(r => r.DeleteBookAsync(command.BookId)).ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}










