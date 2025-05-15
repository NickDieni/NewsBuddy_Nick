using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Storage;
using NewsAPI.Models;
using NewsBuddy_Nick.Helpers; // Assuming ArticleOptions lives here
using System.Text.Json;

namespace NewsBuddy_Nick.Pages;

public class Favorite : ComponentBase
{
    protected List<Article> favorites = new();
    protected List<Article> articles = new();

    protected override void OnInitialized()
    {
        LoadFavorites();
        articles = favorites; // Ensure articles list is initialized too
    }

    protected void LoadFavorites()
    {
        string json = Preferences.Get("favorite_articles", "[]");

        try
        {
            favorites = JsonSerializer.Deserialize<List<Article>>(json) ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading favorites: {ex.Message}");
            favorites = new();
        }
    }

    protected async Task OpenArticle(string url)
    {
        try
        {
            await Launcher.Default.OpenAsync(url);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not open URL: {ex.Message}");
        }
    }

    protected async Task ShowOptions(Article article)
    {
        if (articles == null)
        {
            Console.WriteLine("Articles list is null.");
            return;
        }

        await ArticleOptions.ShowOptionsAsync(article, articles, StateHasChanged);
    }
}
