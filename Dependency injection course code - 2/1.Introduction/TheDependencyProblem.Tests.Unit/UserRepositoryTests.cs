using System.Linq;
using System.Threading.Tasks;
using Moq;
using NSubstitute;
using TheDependencyProblem.Data;
using Xunit;

namespace TheDependencyProblem.Tests.Unit;

public class UserRepositoryTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public UserRepositoryTests()
    {
        //_userService = new UserService(new FakeUserRepository());
        _userService = new UserService(_mockUserRepository.Object);
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        _mockUserRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(Enumerable.Empty<User>());
        
        // Act

        // Assert

    }
}
