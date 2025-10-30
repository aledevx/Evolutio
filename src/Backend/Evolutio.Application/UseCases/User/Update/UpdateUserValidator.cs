using Evolutio.Communication.Requests;
using Evolutio.Exception;
using FluentValidation;

namespace Evolutio.Application.UseCases.User.Update;
public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        When(user => String.IsNullOrWhiteSpace(user.Email) is false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
        RuleFor(user => user.Perfil).IsInEnum().WithMessage(ResourceMessagesException.PROFILE_INVALID);
    }
}

