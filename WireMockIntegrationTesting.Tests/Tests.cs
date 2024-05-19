using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMockIntegrationTesting.Cli;

namespace WireMockIntegrationTesting.Tests;

public class Tests
{
    private static readonly int[] sampleTopStories = [1, 2, 3];

    [Test]
    public async Task MockApiClient()
    {
        var apiClient = new Mock<IApiClient>();
        apiClient.Setup(x => x.GetTopStoriesAsync(It.IsAny<Uri>())).ReturnsAsync(sampleTopStories);
        var appCommand = new AppCommand(apiClient.Object)
        {
            BaseUrl = new Uri("https://example.com")
        };

        var result = await appCommand.OnExecuteAsync();

        result.Should().Be(0);
    }

    [Test]
    public async Task MockHttpClient()
    {
        var baseUrl = new Uri("https://hacker-news.firebaseio.com");
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .SetupRequest(HttpMethod.Get, $"{baseUrl}v0/topstories.json")
            .ReturnsJsonResponse(sampleTopStories);
        var httpClient = handlerMock.CreateClient();
        var apiClient = new ApiClient(httpClient);

        var topStories = await apiClient.GetTopStoriesAsync(baseUrl);

        topStories.Should().BeEquivalentTo(sampleTopStories);
    }

    [Test]
    public void MockApi()
    {
        var server = WireMockServer.Start();
        try
        {
            server
                .Given(Request.Create().WithPath("/v0/topstories.json"))
                .RespondWith(Response.Create().WithBodyAsJson(sampleTopStories));
            var entryPoint = typeof(AppCommand).Assembly.EntryPoint!;
            string[] args = ["--base-url", server.Urls[0]];

            var result = entryPoint.Invoke(null, [args]);

            result.Should().Be(0);
        }
        finally
        {
            server.Stop();
        }
    }
}
