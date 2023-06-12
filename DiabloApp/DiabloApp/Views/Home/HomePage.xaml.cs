using DiabloApp.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabloApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : FlyoutPage
    {
        public HomePage()
        {
            InitializeComponent();
            this.FlyoutMenu.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is FlyoutMenuItem item))
            {
                 return;
            }

            Page page = Activator.CreateInstance(item.TargetType) as Page;
            page.Title = item.Title;

            this.Detail = new NavigationPage(page);
            this.IsPresented = false;
            this.FlyoutMenu.ListView.SelectedItem = null;
        }
    }
}