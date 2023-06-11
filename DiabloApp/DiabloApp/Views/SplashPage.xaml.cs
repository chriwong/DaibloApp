using DiabloApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabloApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            this.ImageSplash.Source = ImageSource.FromResource("DiabloApp.Assets.LandingBackground.jpg");
        }
    }
}