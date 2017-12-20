using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scanner : ContentPage
    {
        //Initialize the tools for the scanner
        private ZXingScannerView zxing;
        private ZXingDefaultOverlay overlay;

        public Scanner()
        {
            InitializeComponent();

            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    zxing.IsAnalyzing = false;

                    // Show an alert
                    //await DisplayAlert("Scanned Barcode", result.Text, "OK");

                    // Navigate away
                    //await Navigation.PopAsync(true);
                    await Navigation.PushAsync(new Info(result.Text), true);
                });

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = zxing.HasTorch,
            };
            overlay.FlashButtonClicked += (sender, e) =>
            {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };
            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            // The root page of your application
            Content = grid;
        }

        // This will be deleted with the button in XAML
        async void OnButtonClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new Info(), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
            zxing.IsAnalyzing = true;

            var contentHolder = Content;
            Content = null;
            Content = contentHolder;
        }

        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;
            zxing.IsAnalyzing = false;

            base.OnDisappearing();
        }
    }
}