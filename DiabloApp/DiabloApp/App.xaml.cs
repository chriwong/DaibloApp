using DiabloApp.ViewModels;
using DiabloApp.ViewModels.Home;
using DiabloApp.Views;
using DiabloApp.Views.Home;
using Prism;
using Prism.Ioc;
using System;

namespace DiabloApp
{
    public partial class App : Prism.DryIoc.PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            var result = this.NavigationService.NavigateAsync("SplashPage");
            if (result.IsFaulted)
            {
                Console.WriteLine(result.Exception.ToString());
            }
        }

        // Prism navigation step 1: register pages
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>(); // optionally give it a different identifier than the view class name
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
