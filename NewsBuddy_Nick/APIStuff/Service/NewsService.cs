using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace NewsBuddy_Nick.Services;

public class NewsService
{
    private readonly NewsApiClient _newsApiClient;

    public NewsService()
    {
        _newsApiClient = new NewsApiClient("dada209737d6415398a1963568ec724a");
    }

    public async Task<List<Article>> GetNewsAsync()
    {
        var request = new EverythingRequest
        {
            Q = "News",
            SortBy = SortBys.Popularity,
            Language = Languages.DA,
            From = DateTime.Today.AddDays(-7)
        };

        // NewsAPI is not async but can be wrapped
        return await Task.Run(() =>
        {
            var result = _newsApiClient.GetEverything(request);
            return result.Status == Statuses.Ok ? result.Articles : new List<Article>();
        });
    }
}
