using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Storage;
using NewsAPI.Models;
using System.Text.Json;

namespace NewsBuddy_Nick.Pages;

public class FavoriteBase : ComponentBase
{
    protected List<Article>? favorites;

    protected override void OnInitialized()
    {
        LoadFavorites();
    }

    protected void LoadFavorites()
    {
        string json = Preferences.Get("favorite_articles", "[]");
        favorites = JsonSerializer.Deserialize<List<Article>>(json) ?? new();
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
}
