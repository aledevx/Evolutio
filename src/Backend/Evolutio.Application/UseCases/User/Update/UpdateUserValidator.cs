using Evolutio.Communication.Constants;
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
        RuleFor(user => user.Perfil).NotEmpty().WithMessage(ResourceMessagesException.PROFILE_EMPTY);
        When(user => String.IsNullOrEmpty(user.Perfil) is false, () =>
        {
            RuleFor(user => user.Perfil).Must(p => Perfil.Todos.Contains(p)).WithMessage(ResourceMessagesException.PROFILE_INVALID);
        });
    }
}

