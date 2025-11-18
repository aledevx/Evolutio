namespace Evolutio.Communication.Responses;
public class ResponseDatabaseStatusJson
{
    public string Status { get; set; } = string.Empty;
    public IList<ResponseCheckJson> Checks { get; set; } = [];
    public double TotalDuration { get; set; }
}
