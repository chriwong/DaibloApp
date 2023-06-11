using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiabloApp.ViewModels.Home
{
    internal class HomePageViewModel : ReactiveObject
    {
        [Reactive]
        public string FlyoutHeader { get; set; }

        public HomePageViewModel()
        {
            FlyoutHeader = "Welcome to Sanctuary";
        }
    }
}
