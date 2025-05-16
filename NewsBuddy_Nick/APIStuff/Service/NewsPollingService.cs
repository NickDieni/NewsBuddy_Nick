using NewsAPI.Models;
using NewsBuddy_Nick.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBuddy_Nick.APIStuff.Service
{
    public class NewsPollingService
    {
        private readonly NewsService _newsService;
        private PeriodicTimer _timer;
        private CancellationTokenSource _cts;
        private List<Article> _lastFetchedArticles = new();
        private string _keyword = "news";  // default keyword

        public NewsPollingService(NewsService newsService)
        {
            _newsService = newsService;
        }

        // Add this method to fix the error
        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
        }


        public void Start()
        {
            _cts = new CancellationTokenSource();
            _timer = new PeriodicTimer(TimeSpan.FromMinutes(10));

            Task.Run(async () =>
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    await CheckForNewNewsAsync();
                }
            });
        }

        public void Stop()
        {
            _cts?.Cancel();
            _timer?.Dispose();
        }

        private async Task CheckForNewNewsAsync()
        {
            var keyword = Preferences.Get("news_keyword", "News");
            var latestArticles = await _newsService.GetLatestNewsWithKeywordAsync(keyword);


            var newArticles = latestArticles
                .Where(a => !_lastFetchedArticles.Any(old => old.Url == a.Url))
                .ToList();

            if (newArticles.Any())
            {
                ShowNotification(newArticles.Count);
                _lastFetchedArticles = latestArticles;
            }
        }
       



        private void ShowNotification(int newCount)
        {
            var notification = new Plugin.LocalNotification.NotificationRequest
            {
                NotificationId = 100,
                Title = "NewsBuddy",
                Description = $"{newCount} new articles available!"
            };

            Plugin.LocalNotification.LocalNotificationCenter.Current.Show(notification);
        }
    }
}
