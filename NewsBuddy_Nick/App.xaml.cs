using Microsoft.Extensions.DependencyInjection;
using NewsBuddy_Nick.APIStuff.Service;

namespace NewsBuddy_Nick
{
    public partial class App : Application
    {
        private readonly NewsPollingService _pollingService;

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new MainPage(); // only this line sets the main page

            _pollingService = serviceProvider.GetRequiredService<NewsPollingService>();
            _pollingService.Start();
        }

        protected override void OnStart()
        {
            base.OnStart();
            _pollingService.Start();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            _pollingService.Stop();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _pollingService.Start();
        }

        // Remove this method completely:
        // protected override Window CreateWindow(IActivationState? activationState)
        // {
        //     return new Window(new MainPage()) { Title = "NewsBuddy_Nick" };
        // }
    }

}
