﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NewsAPI.Models;
using NewsBuddy_Nick.APIStuff.Models;
using NewsBuddy_Nick.Helpers;
using NewsBuddy_Nick.Services;
using System.Text.Json;

namespace NewsBuddy_Nick.Pages;

public partial class Home : ComponentBase
{
    protected List<Article>? articles;

    // Injected services
    [Inject] public NewsService NewsService { get; set; } = default!;
    [Inject] public IJSRuntime JS { get; set; } = default!;

    private DotNetObjectReference<Home>? dotNetRef;

    protected override async Task OnInitializedAsync()
    {
        articles = await NewsService.GetNewsAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && articles != null)
        {
            dotNetRef = DotNetObjectReference.Create(this);
            foreach (var article in articles)
            {
                var id = $"article_{article.Title.GetHashCode()}";
                await JS.InvokeVoidAsync("addLongPressListener", id, dotNetRef);
            }
        }
    }

    protected async Task OpenArticle(string url)
    {
        await Launcher.Default.OpenAsync(url);
    }

    [JSInvokable]
    public async Task OnLongPress(string elementId)
    {
        var article = articles?.FirstOrDefault(a => $"article_{a.Title.GetHashCode()}" == elementId);
        if (article == null) return;

        var action = await App.Current.MainPage.DisplayActionSheet(
            "Options", "Cancel", null, "Favorite", "Delete");

        if (action == "Favorite")
        {
            Console.WriteLine($"Favorited: {article.Title}");
        }
        else if (action == "Delete")
        {
            articles.Remove(article);
            StateHasChanged();
        }
    }
    protected async Task ShowOptions(Article article)
    {
        await ArticleOptions.ShowOptionsAsync(article, articles!, StateHasChanged);
    }





}
