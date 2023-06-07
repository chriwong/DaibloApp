using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DiabloApp.ViewModels
{
    public class LandingPageViewModel : BindableObject
    {
        private int _count = 1;
        private string _counterText = "Click me to count up from 1!";
        public string CounterText
        {
            get { return _counterText; }
            set
            {
                if (_counterText != value)
                {
                    System.Console.WriteLine("\n\nProperty changed\n\n");
                    _counterText = value;
                    OnPropertyChanged(nameof(CounterText));
                }
            }
        }
        public ICommand IncrementCommand { get; }   // you can't set default values for non-static fields; they must be set in the ctor

        public LandingPageViewModel()
        {
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
