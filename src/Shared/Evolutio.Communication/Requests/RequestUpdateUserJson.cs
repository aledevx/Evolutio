using Evolutio.Communication.Enums;

namespace Evolutio.Communication.Requests;
public class RequestUpdateUserJson
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Perfil Perfil { get; set; }
}

