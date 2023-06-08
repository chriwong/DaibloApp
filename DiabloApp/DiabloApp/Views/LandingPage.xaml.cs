using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            this.LandingImage.Source = ImageSource.FromResource("DiabloApp.Assets.LandingBackground.jpg");
        }

        private void ProceedButton_Clicked(object sender, EventArgs e)
        {
            this.ProceedButton.Text= "TODO";
        }
    }
}