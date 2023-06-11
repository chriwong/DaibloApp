using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiabloApp.ViewModels
{
    public class SplashPageViewModel : ReactiveObject
    {
        private readonly INavigationService _navigationService;

        private int _count = 1;
        
        [Reactive]
        public string CounterText { get; set; }

        public ICommand IncrementCommand { get; }   // you can't set default values for non-static fields; they must be set in the ctor
        public ICommand ProceedToHomeCommand { get; }

        public SplashPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;
            this.CounterText = "Click to count!";
            this.IncrementCommand = new Command<string>(this.IncrementCounter);
            this.ProceedToHomeCommand = new Command(this.NavigateToHome);
        }

        private void IncrementCounter(string s)
        {
            int n = int.Parse(s);
            this._count += n;
            this.CounterText = $"Count: {this._count}";
        }

        private async void NavigateToHome()
        {
            var result = await this._navigationService.NavigateAsync("HomePage");
            if (!result.Success)
            {
                Console.WriteLine(result.Exception);
            }
        }
    }

}
