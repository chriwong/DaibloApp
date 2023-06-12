using DiabloApp.Models;
using DiabloApp.Views.Home;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace DiabloApp.ViewModels.Home
{
    public class HomeFlyoutViewModel : ReactiveObject
    {
        // ---
        // TODO use DynamicData observable collection instead of .NET observable collection
        // ---
        [Reactive]
        public string MenuTitle { get; set; }

        public ObservableCollection<FlyoutMenuItem> MenuItems { get; set; }

        public HomeFlyoutViewModel()
        {
            this.MenuTitle = "Welcome to Sanctuary";
            this.MenuItems = new ObservableCollection<FlyoutMenuItem>(new[]
            {
                new FlyoutMenuItem { Id = 0, Title = "About the Game" },
                new FlyoutMenuItem { Id = 1, Title = "Release Info" },
                new FlyoutMenuItem { Id = 2, Title = "Classes" },
                new FlyoutMenuItem { Id = 3, Title = "Items" },
                new FlyoutMenuItem { Id = 4, Title = "Campaign Players" },
                new FlyoutMenuItem { Id = 5, Title = "App Info" },
            });
        }
    }
}
