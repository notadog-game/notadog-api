using FluentValidation;
using NotadogApi.Models;

using NotadogApi.Domain.Users.Services;

public class UserLoginDtoModelValidator : AbstractValidator<UserLoginDto>
{
    private readonly IUserService _userService;

    public UserLoginDtoModelValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellation) =>
            {
                var existingUser = await _userService.GetOneByEmailAsync(email);
                return existingUser != null;
            });

        RuleFor(user => user.Password).NotEmpty();
        RuleFor(user => user)
            .MustAsync(async (user, cancellation) =>
            {
                var existingUser = await _userService.GetOneByEmailAsync(user.Email);
                return existingUser.Password == user.Password;
            });
    }
}