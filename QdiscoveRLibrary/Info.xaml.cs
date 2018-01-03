using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using QdiscoveR.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Net.Http;

namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {

        public static ICollection<Building> SimilarBuildingsOc = new ObservableCollection<Building>();

        private IMobileServiceTable<Building> _buildingTable;
        private Position _userPos;

        //Our constant for the distance calculation
        private const double Dist = 1.5;

        private string buildingId;

        public Info(string buildingId)
        {
            InitializeComponent();

            this.buildingId = buildingId;

            //Using this way beacause constructor of a page cannot be async (only in C# 7.1)
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    _buildingTable = App.MobileService.GetTable<Building>();
                    var buildingItem = await _buildingTable.Where(x => (x.id == buildingId)).ToListAsync();
                    var building = buildingItem.FirstOrDefault();

                    FindSimilarBuildings(_buildingTable, buildingId, _userPos);

                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;

                    if (building != null)
                    {
                        BName.Text = building.Name;
                        BInfo.Text = building.Info;
                        BLocation.Text = building.Lat + ", " + building.Lng;

                        // Popoulating the list of buildings nearby
                        SimilarBuildings.ItemsSource = SimilarBuildingsOc;
                    }
                    else
                    {
                        BName.Text = "Cannot find a building";
                    }
                }               
                catch (MobileServiceInvalidOperationException ex)
                {
                    await DisplayAlert("Error", "A service related issue occured. Please contact admin.","Ok");
                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;
                }
                catch (Exception ex)
                {
                    if (ex.ToString().Contains("HttpRequestException"))
                    {
                        await DisplayAlert("Error", "An Network error occured. Please check network connectivity.", "Ok");
                        ActivityIndicator.IsRunning = false;
                        ActivityIndicator.IsVisible = false;
                    }                
                    else
                    {
                        await DisplayAlert("Error", "An error occured please try again", "Ok");
                        ActivityIndicator.IsRunning = false;
                        ActivityIndicator.IsVisible = false;
                    }
                }
                //Exception in this query, the error tha you cant use more function than those described here https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-how-to-use-client-library#filtering

                //SimilarBuildingsOc = await buildingTable.Where(x => x.id != buildingId && (3956 * 2 * Math.Asin((Math.Sqrt(Math.Pow(
                //    Math.Sin((userPos.Latitude - Math.Abs(x.Lat)) * Math.PI / 180 / 2), 2) + Math.Cos(userPos.Latitude * Math.PI / 180)
                //    * Math.Cos(Math.Abs(x.Lat) * Math.PI / 180) * Math.Pow(Math.Sin((userPos.Longitude - x.Lng) * Math.PI / 180 / 2), 2))))
                //    < Dist)).ToListAsync();

                //SimilarBuildingsOc = await buildingTable.OrderBy(x => (x.Lat - userPos.Latitude) * (x.Lat - userPos.Latitude) + (x.Lng-userPos.Longitude)* (x.Lng - userPos.Longitude)).ToListAsync();                

                //TODO remove these, are  for test
                //var lat = 37.796071;
                //var lng = 26.705048;


            });
                       
            //Disable selection of items on the list
            SimilarBuildings.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

        }

        private async void FindSimilarBuildings(IMobileServiceTable<Building> buildingTable, string buildingId, Position userPos)
        {
            var temp = await buildingTable.Where(x => x.id != buildingId).ToListAsync();
            foreach (var x in temp)
            {
                if ((3956 * 2 * Math.Asin((Math.Sqrt(Math.Pow(Math.Sin((userPos.Latitude - Math.Abs(x.Lat))
                    * Math.PI / 180 / 2), 2) + Math.Cos(userPos.Latitude * Math.PI / 180) * Math.Cos(Math.Abs(x.Lat)
                    * Math.PI / 180) * Math.Pow(Math.Sin((userPos.Longitude - x.Lng) * Math.PI / 180 / 2), 2)))) < Dist))
                {
                    SimilarBuildingsOc.Add(x);
                }
            }
        }

        public void OnRefresh(object sender, EventArgs e)
        {            
            FindSimilarBuildings(_buildingTable, buildingId, _userPos);
        }

        public async Task<Position> GetCurrentLocation()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                //Display error as we have timed out or can't get location.
                await DisplayAlert("Location", "Location serrvice timed out. Please try again", "Ok");
                return null;
            }

            //var output =$"Time: {position.Timestamp} \nLat: {position.Latitude} \nLong: {position.Longitude} \nAltitude:" +
            //            $" {position.Altitude} \nAltitude Accuracy: {position.AltitudeAccuracy} \nAccuracy: {position.Accuracy}" +
            //            $" \nHeading: {position.Heading} \nSpeed: {position.Speed}";

            //Debug.WriteLine(output);

            return position;
        }

    }
}