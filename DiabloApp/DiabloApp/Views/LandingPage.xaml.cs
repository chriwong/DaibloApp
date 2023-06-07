using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabloApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            this.ImageLanding.Source = ImageSource.FromResource("DiabloApp.Assets.LandingBackground.jpg");
        }
    }
}