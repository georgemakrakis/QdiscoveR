using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace QdiscoveR
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //Remove the bar from the first page
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new Scanner(), true);

            //This line used for debugging if device camera does not exist
            await Navigation.PushAsync(new Info("EFE58D97-7233-469D-95E6-EADF7C17B1B4"), true);
        }
    }
}
