using Microsoft.Extensions.Logging;
using NewsBuddy_Nick.APIStuff.Service;
using NewsBuddy_Nick.Services;
using Plugin.LocalNotification;

namespace NewsBuddy_Nick;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<NewsService>();
        builder.Services.AddSingleton<NewsPollingService>();

        #if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        #endif

        return builder.Build();
    }
}
