using Evolutio.Domain.Enums;

namespace Evolutio.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Perfil Perfil { get; set; } = Perfil.Funcionario;
    public Guid UserIdentifier { get; set; } = Guid.NewGuid();
}

