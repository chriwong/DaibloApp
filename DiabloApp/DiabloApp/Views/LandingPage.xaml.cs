using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabloApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        private int _count = 1;
        private string _counterText = "Click me to count up from 2!";
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

        public LandingPage()
        {
            InitializeComponent();
            this.ImageLanding.Source = ImageSource.FromResource("DiabloApp.Assets.LandingBackground.jpg");
            this.IncrementCommand = new Command<string>(this.IncrementCounter);
            this.BindingContext = this;
        }

        private void IncrementCounter(string s)
        {
            System.Console.WriteLine($"\nIncrementing by {s}, which is a: {s.GetType().FullName}\n");
            int n = int.Parse(s);
            this._count += n;
            this.CounterText = $"Count: {this._count}";
        }

    }
}