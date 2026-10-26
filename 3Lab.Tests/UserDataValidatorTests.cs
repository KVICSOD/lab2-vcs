using AuthSystem;

namespace AuthSystem.Tests;

public class UserDataValidatorTests
{
    private readonly UserDataValidator _validator = new();

    [Fact]
    public void Valid_User_Should_Pass()
    {
        var result = _validator.Validate(new UserData
        {
            Email = "test@mail.ru",
            Phone = "+375291946690",
            Password = "12345678",
            Age = 20
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Invalid_Email_Should_Fail()
    {
        var result = _validator.Validate(new UserData
        {
            Email = "testmail.ru",
            Phone = "+375291946690",
            Password = "12345678",
            Age = 20
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Некорректный формат Email");
    }

    [Fact]
    public void Age_Below_18_Should_Fail()
    {
        var result = _validator.Validate(new UserData
        {
            Email = "test@mail.ru",
            Phone = "+375291946690",
            Password = "12345678",
            Age = 16
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "Возраст должен быть 18+");
    }
}
