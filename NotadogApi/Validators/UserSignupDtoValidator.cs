using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;

public class UserSignupDtoValidator : AbstractValidator<UserSignupDto>
{
    public UserSignupDtoValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(userDto => userDto.Name).SetValidator(new UserNameValidator());
        RuleFor(userDto => userDto.Email).SetValidator(new UserEmailValidator());
        RuleFor(userDto => userDto.Password).SetValidator(new UserPasswordValidator());
    }
}