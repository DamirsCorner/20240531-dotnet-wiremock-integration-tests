using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WireMockIntegrationTesting.Cli;

return await Host.CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .RunCommandLineApplicationAsync<AppCommand>(args)
    .ConfigureAwait(false);

static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services.AddTransient<IApiClient, ApiClient>();
    services.AddHttpClient<ApiClient>();
}
