
public interface IApiClient
{
    Task<int[]> GetTopStoriesAsync(Uri baseUrl);
}