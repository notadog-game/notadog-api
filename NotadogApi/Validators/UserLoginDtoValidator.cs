using System;
using System.Web;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

using FluentValidation;
using NotadogApi.Models;
using NotadogApi.Domain.Exceptions;
using NotadogApi.Domain.Users.Models;

using NotadogApi.Domain.Users.Services;

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        CascadeMode = CascadeMode.StopOnFirstFailure;

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