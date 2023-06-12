using DiabloApp.ViewModels.Home;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabloApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeFlyout : ContentPage
    {
        public ListView ListView;

        public HomeFlyout()
        {
            InitializeComponent();

            this.BindingContext = new HomeFlyoutViewModel();
            this.ListView = MenuItemsListView;
        }

    }
}