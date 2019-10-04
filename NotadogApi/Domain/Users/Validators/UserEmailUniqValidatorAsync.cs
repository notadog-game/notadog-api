using FluentValidation;
using NotadogApi.Domain.Exceptions;
using NotadogApi.Domain.Users.Services;

namespace NotadogApi.Domain.Users.Validators
{
    public class UserEmailUniqValidatorAsync : AbstractValidator<string>
    {
        public UserEmailUniqValidatorAsync(IUserService userService)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(email => email).MustAsync(async (email, cancellation) =>
            {
                var user = await userService.GetOneByEmailAsync(email);
                return user == null;
            }).OnFailure(email =>
            {
                throw new CommonException(ErrorCode.UserEmailAlreadyExist);
            });
        }
    }
}