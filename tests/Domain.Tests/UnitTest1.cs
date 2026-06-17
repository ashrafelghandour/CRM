using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_should_CreateUserSuccessfully()
    {
        // Arrange
        var user = new User
        {
            Id = 1234,
            Username = "ashrafmohamed",
            Email = "ashraf5912@example.com",
            PassworedHash = "hashed_password",
            FirstName = "Ashraf",
            LastName = "Mohamed",
            Phone = "1234567890",
            Role = Domain.Enums.UserRole.Customer,
            LastLogin = DateTime.UtcNow,
        };

        // Act & Assert
        user.Id.Should().Be(1234);
        user.Username.Should().Be("ashrafmohamed");

        
    }
    [Fact]
     public void CreateSuperAdminUser_should_createwithRoleSuperAdmin()
    {
        //arrange & Act
        var superAdmin = new SuperAdmin();
        //Assert
        superAdmin.Role.Should().Be(Enums.UserRole.SuperAdmin);

    }

    // [Theory]
    // [InlineData("", "Ashraf","ashraf59121718@gmail.com","FirstName cannot be empty")]
    // [InlineData("Ashraf", "","ashraf59121718@gmail.com","LastName cannot be empty")]
    // [InlineData("Ashraf", "Ashraf","ashraf59121718gmail.com","email must contain @")]
    // public void UserEntity_WhenInvalidData_ShouldThrowException(string FirstName,string LastName, string email,string Expected)
    // {
    //     //Arrange & act
    //     var systemUser = new SystemUser
    //     {
    //         FirstName = FirstName,
    //         LastName = LastName,
    //         Email = email
    //     };
    //     //Assert
    //     systemUser.FirstName.Should().NotBeNullOrEmpty(Expected);
    //     systemUser.Email.Should().Contain("@", Expected);
    //     systemUser.LastName.Should().NotBeNullOrEmpty(Expected);
    // }

}
