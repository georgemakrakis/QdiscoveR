using System;
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
            SizeChanged += MainPageSizeChanged;
        }

        void MainPageSizeChanged(object sender, EventArgs e)
        {
            //Changing the size of the logo in landscape mode so it could fit better
            bool isPortrait = Height > Width;
            Logo.HeightRequest = (isPortrait? Math.Min(Height, 312) : Math.Min(Height, 250));
        }
        
        async void OnButtonClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new Scanner(), true);

            //This line used for debugging if device camera does not exist
            await Navigation.PushAsync(new Info("EFE58D97-7233-469D-95E6-EADF7C17B1B4"), true);
        }
    }
}
