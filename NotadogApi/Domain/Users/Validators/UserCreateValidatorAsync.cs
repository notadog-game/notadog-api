using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Users.Services;

namespace NotadogApi.Domain.Users.Validators
{
    public class UserCreateValidatorAsync : AbstractValidator<UserCreateDto>
    {
        public UserCreateValidatorAsync(IUserService userService)
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
}