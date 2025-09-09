using Evolutio.Application.SharedValidators;
using Evolutio.Communication.Requests;
using Evolutio.Exception;
using FluentValidation;

namespace Evolutio.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator() 
    {
        //Valida se o nome não é vazio
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        //Valida se o email não é vazio
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
        //Valida se o email é válido, primeiro verificando se o email não é nulo ou vazio
        When(user => String.IsNullOrWhiteSpace(user.Email) is false, () =>
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
        });
        //Valida se a senha segue os padrões de segurança definidos
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}

