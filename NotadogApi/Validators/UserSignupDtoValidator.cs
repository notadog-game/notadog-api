using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;

public class UserSignupDtoValidator : AbstractValidator<UserSignupDto>
{
    public UserSignupDtoValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(userDto => userDto.Name)
            .NotEmpty()
            .OnFailure(dto =>
            {
                throw new CommonException(ErrorCode.UserNameMustNotBeEmpty);
            });

        RuleFor(userDto => userDto.Email)
            .NotEmpty()
            .OnFailure(dto =>
            {
                throw new CommonException(ErrorCode.UserEmailMustNotBeEmpty);
            })
            .EmailAddress()
            .OnFailure(dto =>
            {
                throw new CommonException(ErrorCode.UserEmailIsNotValid);
            });

        RuleFor(userDto => userDto.Password)
            .NotEmpty()
            .OnFailure(dto =>
            {
                throw new CommonException(ErrorCode.UserPasswordMustNotBeEmpty);
            });
    }
}