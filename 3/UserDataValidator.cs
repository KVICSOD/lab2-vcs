using FluentValidation;

namespace AuthSystem;

public class UserDataValidator : AbstractValidator<UserData>
{
    public UserDataValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email не может быть пустым")
            .EmailAddress().WithMessage("Некорректный формат Email");

        RuleFor(u => u.Phone)
            .NotEmpty().WithMessage("Телефон не может быть пустым")
            .MinimumLength(10).WithMessage("Телефон должен содержать минимум 10 символов");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Пароль не может быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов");

        RuleFor(u => u.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Возраст должен быть 18+");
    }
}
