using NewsAPI.Models;
using NewsBuddy_Nick.APIStuff.Models;
using System.Text.Json;

namespace NewsBuddy_Nick.Helpers
{
    public static class ArticleOptions
    {
        public static async Task ShowOptionsAsync(Article article, List<Article> currentArticles, Action refreshUI)
        {
            string action = await App.Current.Windows[0].Page.DisplayActionSheet(
                "Options", "Cancel", null, "Favorite", "Delete");

            if (action == "Favorite")
            {
                await ToggleFavoriteAsync(article, refreshUI);
            }
            else if (action == "Delete")
            {
                await DeleteArticleAsync(article, currentArticles, refreshUI);
            }
        }

        public static async Task ToggleFavoriteAsync(Article article, Action? refreshUI = null)
        {
            string json = Preferences.Get("favorite_articles", "[]");
            var list = JsonSerializer.Deserialize<List<Article>>(json) ?? new();

            var existing = list.FirstOrDefault(a => a.Url == article.Url);
            if (existing != null)
            {
                list.Remove(existing);
                await App.Current.Windows[0].Page.DisplayAlert("Removed", "Removed from favorites.", "OK");
            }
            else
            {
                list.Add(article);
                await App.Current.Windows[0].Page.DisplayAlert("Saved", "Added to favorites.", "OK");
            }

            Preferences.Set("favorite_articles", JsonSerializer.Serialize(list));
            refreshUI?.Invoke();
        }

        public static async Task DeleteArticleAsync(Article article, List<Article> currentArticles, Action refreshUI)
        {
            var itemToRemove = currentArticles.FirstOrDefault(a => a.Url == article.Url);
            if (itemToRemove != null)
            {
                currentArticles.Remove(itemToRemove);
            }

            var json = Preferences.Get("favorite_articles", "[]");
            var list = JsonSerializer.Deserialize<List<Article>>(json) ?? new();
            list.RemoveAll(a => a.Url == article.Url);
            Preferences.Set("favorite_articles", JsonSerializer.Serialize(list));

            refreshUI();
        }

        public static bool IsFavorite(Article article)
        {
            var json = Preferences.Get("favorite_articles", "[]");
            var list = JsonSerializer.Deserialize<List<Article>>(json) ?? new();
            return list.Any(a => a.Url == article.Url);
        }
    }
}
