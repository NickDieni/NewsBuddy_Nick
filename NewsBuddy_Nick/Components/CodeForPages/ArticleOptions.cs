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
                await SaveFavoriteAsync(article);
            }
            else if (action == "Delete")
            {
                // Remove from both currentArticles and from Preferences
                var itemToRemove = currentArticles.FirstOrDefault(a => a.Url == article.Url);
                if (itemToRemove != null)
                {
                    currentArticles.Remove(itemToRemove);

                    // 🔁 Update Preferences as well
                    var storedJson = Preferences.Get("favorite_articles", "[]");
                    var storedList = JsonSerializer.Deserialize<List<Article>>(storedJson) ?? new();
                    var storedItem = storedList.FirstOrDefault(a => a.Url == article.Url);
                    if (storedItem != null)
                    {
                        storedList.Remove(storedItem);
                        Preferences.Set("favorite_articles", JsonSerializer.Serialize(storedList));
                    }

                    refreshUI(); // Refresh UI after modification
                }
            }
        }


        public static async Task SaveFavoriteAsync(Article article)
        {
            var json = Preferences.Get("favorite_articles", "[]");
            var list = JsonSerializer.Deserialize<List<FavoriteArticle>>(json) ?? new();

            if (!list.Any(a => a.Url == article.Url))
            {
                list.Add(new FavoriteArticle
                {
                    Title = article.Title,
                    Author = article.Author,
                    Description = article.Description,
                    Url = article.Url
                });

                Preferences.Set("favorite_articles", JsonSerializer.Serialize(list));
                await App.Current.Windows[0].Page.DisplayAlert("Saved", "Article added to favorites.", "OK");
            }
            else
            {
                await App.Current.Windows[0].Page.DisplayAlert("Already Saved", "This article is already in your favorites.", "OK");
            }
        }
    }
}
