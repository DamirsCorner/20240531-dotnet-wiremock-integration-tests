using System.Net.Http.Json;

namespace WireMockIntegrationTesting.Cli;

public class ApiClient(HttpClient httpClient) : IApiClient
{
    public async Task<int[]> GetTopStoriesAsync(Uri baseUrl)
    {
        var response = await httpClient.GetAsync($"{baseUrl}v0/topstories.json");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int[]>() ?? [];
    }
}
