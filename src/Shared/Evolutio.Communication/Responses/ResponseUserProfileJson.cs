using Evolutio.Communication.Enums;

namespace Evolutio.Communication.Responses;
public class ResponseUserProfileJson
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Perfil Perfil { get; set; }
    public bool Active { get; set; }
    }

