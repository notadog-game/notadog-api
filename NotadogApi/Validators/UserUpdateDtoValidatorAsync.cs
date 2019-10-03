using FluentValidation;
using NotadogApi.Domain.Users.Models;
using NotadogApi.Domain.Users.Services;

public class UserUpdateDtoValidatorAsync : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidatorAsync(IUserService userService)
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(userDto => userDto.Name)
            .SetValidator(new UserNameValidator())
            .When(userDto => userDto.Name != null);

        RuleFor(userDto => userDto.Email)
            .SetValidator(new UserEmailValidator())
            .SetValidator(new UserEmailUniqValidatorAsync(userService))
            .When(userDto => userDto.Email != null);

        RuleFor(userDto => userDto.Password)
            .SetValidator(new UserPasswordValidator())
            .When(userDto => userDto.Password != null);
    }
}