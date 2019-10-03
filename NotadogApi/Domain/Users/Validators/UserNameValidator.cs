using FluentValidation;
using NotadogApi.Domain.Exceptions;

namespace NotadogApi.Domain.Users.Validators
{
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
}