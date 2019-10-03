using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Users.Services;

public class UserSignupDtoValidatorAsync : AbstractValidator<UserSignupDto>
{
    public UserSignupDtoValidatorAsync(IUserService userService)
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(userDto => userDto.Name)
            .SetValidator(new UserNameValidator());

        RuleFor(userDto => userDto.Email)
            .SetValidator(new UserEmailValidator())
            .SetValidator(new UserEmailUniqValidatorAsync(userService));

        RuleFor(userDto => userDto.Password)
            .SetValidator(new UserPasswordValidator());
    }
}