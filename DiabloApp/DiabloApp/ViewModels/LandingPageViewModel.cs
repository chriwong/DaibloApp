using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiabloApp.ViewModels
{
    public class LandingPageViewModel : ReactiveObject
    {
        private int _count = 1;
        
        [Reactive]
        public string CounterText { get; set; }

        public ICommand IncrementCommand { get; }   // you can't set default values for non-static fields; they must be set in the ctor

        public LandingPageViewModel()
        {
            this.CounterText = "Click to count!";
            this.IncrementCommand = new Command<string>(this.IncrementCounter);
        }

        private void IncrementCounter(string s)
        {
            int n = int.Parse(s);
            this._count += n;
            this.CounterText = $"Count: {this._count}";
        }
    }

}
