using Evolutio.Communication.Responses;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace Evolutio.Web.Handlers;
public class HandlerBase
{
    private readonly HttpClient _httpClient;
    public HandlerBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    protected async Task<object> SendRequestWithCredentialsAsync<ResponseSuccess>(HttpMethod method, string url, object? request = null) 
    {
        var httpRequest = new HttpRequestMessage(method, url);

        if (request is not null)
            httpRequest.Content = JsonContent.Create(request);

        httpRequest.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        var result = await _httpClient.SendAsync(httpRequest);

        if (result.IsSuccessStatusCode)
        {
            if (typeof(ResponseSuccess) == typeof(bool)) 
            {
                return true;
            }
            var successResponse = await result.Content.ReadFromJsonAsync<ResponseSuccess>();

            return successResponse!;
        }
        else
        {
            var errorResponse = await result.Content.ReadFromJsonAsync<ResponseErrorJson>();
            return errorResponse!;
        }
    }
}
