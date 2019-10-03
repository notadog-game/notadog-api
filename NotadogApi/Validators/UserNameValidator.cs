using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;

public class UserNameValidator : AbstractValidator<string>
{
    public UserNameValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

        RuleFor(name => name)
            .NotEmpty()
            .OnFailure(name =>
            {
                throw new CommonException(ErrorCode.UserNameMustNotBeEmpty);
            });
    }
}