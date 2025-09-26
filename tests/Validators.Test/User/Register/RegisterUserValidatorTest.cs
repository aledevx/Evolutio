﻿using CommonTestUtilities.Requests;
using Evolutio.Application.UseCases.User.Register;
using Evolutio.Exception;
using FluentAssertions;

namespace Validators.Test.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success() 
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }
    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }
    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = request.Email.Replace("@", "");
        // Debugar para ver o valor do email

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }
    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_EMPTY));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLength)
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.PASSWORD_INVALID));
    }
}

