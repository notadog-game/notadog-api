using FluentValidation;
using NotadogApi.Domain.Exceptions;

namespace NotadogApi.Domain.Users.Validators
{
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
}