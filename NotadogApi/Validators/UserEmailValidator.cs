using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;

public class UserEmailValidator : AbstractValidator<string>
{
    public UserEmailValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(email => email)
            .NotEmpty()
            .OnFailure(email =>
            {
                throw new CommonException(ErrorCode.UserEmailMustNotBeEmpty);
            })
            .EmailAddress()
            .OnFailure(email =>
            {
                throw new CommonException(ErrorCode.UserEmailIsNotValid);
            });
    }
}