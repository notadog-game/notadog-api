using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;

public class UserPasswordValidator : AbstractValidator<string>
{
    public UserPasswordValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(password => password)
            .NotEmpty()
            .OnFailure(password =>
            {
                throw new CommonException(ErrorCode.UserPasswordMustNotBeEmpty);
            });
    }
}