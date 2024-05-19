using McMaster.Extensions.CommandLineUtils;

namespace WireMockIntegrationTesting.Cli;

public class AppCommand(IApiClient apiClient)
{
    [Option(Description = "Base URL of the HackerNews API.")]
    public Uri BaseUrl { get; set; } = new Uri("https://hacker-news.firebaseio.com");

    public async Task<int> OnExecuteAsync()
    {
        var topStories = await apiClient.GetTopStoriesAsync(BaseUrl);
        foreach (var storyId in topStories)
        {
            Console.WriteLine(storyId);
        }
        return 0;
    }
}
