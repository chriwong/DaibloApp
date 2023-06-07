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
        //public ICommand IncrementCommand { get; }   // you can't set default values for non-static fields; they must be set in the ctor

        public LandingPage()
        {
            InitializeComponent();
            this.ImageLanding.Source = ImageSource.FromResource("DiabloApp.Assets.LandingBackground.jpg");
            //this.IncrementCommand = new Command<int>(this.IncrementCounter);
            this.BindingContext = this;
        }

        //private void IncrementCounter(int n)
        //{
        //    this._count += n;
        //    this._count++;
        //    this.CounterText = $"Count: {this._count}";
        //}

        private void ButtonOne_Clicked(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("\n\nButton 1 clicked\n\n");
            this._count++;
            this.CounterText = $"Count: {this._count}";
        }
        private void ButtonTwo_Clicked(object sender, System.EventArgs e)
        {
            System.Console.WriteLine("\n\nButton 2 clicked\n\n");
            this._count += 2;
            this.CounterText = $"Count: {this._count}";
        }
    }
}