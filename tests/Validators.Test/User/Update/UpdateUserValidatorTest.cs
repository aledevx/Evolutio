using CommonTestUtilities.Requests;
using Evolutio.Application.UseCases.User.Update;
using Evolutio.Exception;
using FluentAssertions;

namespace Validators.Test.User.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success() 
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Error_Name_Empty() 
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.NAME_EMPTY));
    }
    [Fact]
    public void Error_Email_Empty() 
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_EMPTY));
    }
    [Fact]
    public void Error_Email_Invalid() 
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = request.Email.Replace("@","");

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.EMAIL_INVALID));
    }
    [Fact]
    public void Error_Profile_Invalid()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.Perfil = (Evolutio.Communication.Enums.Perfil)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceMessagesException.PROFILE_INVALID));
    }

}

