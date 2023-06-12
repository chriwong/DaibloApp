using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DiabloApp.ViewModels.Home
{
    internal class HomePageViewModel : ReactiveObject
    {
        [Reactive]
        public string FlyoutHeader { get; set; }

        public HomePageViewModel()
        {
            this.FlyoutHeader = "Welcome to Sanctuary";
        }
    }
}
