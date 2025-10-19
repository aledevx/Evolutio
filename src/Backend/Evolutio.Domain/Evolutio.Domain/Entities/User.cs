using Evolutio.Domain.Constants;

namespace Evolutio.Domain.Entities;

public class User : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; } = Guid.NewGuid();
}

