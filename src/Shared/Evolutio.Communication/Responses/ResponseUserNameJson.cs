namespace Evolutio.Communication.Responses;
public class ResponseUserNameJson 
{
    public string Name { get; set; } = string.Empty;
    public ResponseUserNameJson(string name)
    {
        Name = name;
    }
    public ResponseUserNameJson()
    {
        
    }
}
