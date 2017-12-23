using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using QdiscoveR.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;


namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {

        public static ICollection<Building> SimilarBuildingsOc = new ObservableCollection<Building>();
        private const double Dist = 1.5;
        public Info(string buildingId)
        {
            InitializeComponent();

            //Using this way beacause constructor of a page cannot be async (only in C# 7.1)
            Device.BeginInvokeOnMainThread(async () =>
            {
                var buildingTable = App.MobileService.GetTable<Building>();
                var buildingItem = await buildingTable.Where(x => (x.id == buildingId)).ToListAsync();
                var building = buildingItem.FirstOrDefault();

                var userPos = await GetCurrentLocation();


                //Exception in this query, the error tha you cant use more function than those described here https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-dotnet-how-to-use-client-library#filtering

                //SimilarBuildingsOc = await buildingTable.Where(x => x.id != buildingId && (3956 * 2 * Math.Asin((Math.Sqrt(Math.Pow(
                //    Math.Sin((userPos.Latitude - Math.Abs(x.Lat)) * Math.PI / 180 / 2), 2) + Math.Cos(userPos.Latitude * Math.PI / 180)
                //    * Math.Cos(Math.Abs(x.Lat) * Math.PI / 180) * Math.Pow(Math.Sin((userPos.Longitude - x.Lng) * Math.PI / 180 / 2), 2))))
                //    < Dist)).ToListAsync();

                //SimilarBuildingsOc = await buildingTable.OrderBy(x => (x.Lat - userPos.Latitude) * (x.Lat - userPos.Latitude) + (x.Lng-userPos.Longitude)* (x.Lng - userPos.Longitude)).ToListAsync();                

                //TODO remove these, are  for test
                var lat = 37.796071;
                var lng = 26.705048;

                var temp = await buildingTable.Where(x => x.id != buildingId).ToListAsync();
                foreach (var x in temp)
                {
                    if ((3956 * 2 * Math.Asin((Math.Sqrt(Math.Pow(Math.Sin((lat - Math.Abs(x.Lat))
                        * Math.PI /180 / 2), 2) + Math.Cos(lat * Math.PI / 180)* Math.Cos(Math.Abs(x.Lat)
                        * Math.PI / 180) * Math.Pow(Math.Sin((lng - x.Lng) * Math.PI / 180 / 2),2))))< Dist))
                    {
                        SimilarBuildingsOc.Add(x);
                    }
                }

                //((lat -$user_lat) * (lat -$user_lat)) +((lng - $user_lng)*(lng - $user_lng))
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;

                if (building != null)
                {
                    BName.Text = building.Name;
                    BInfo.Text = building.Info;
                    BLocation.Text = building.Lat + ", " + building.Lng;
                }
                else
                {
                    BName.Text = "Cannot find a building";
                }
            });

            // Popoulating the list        
            SimilarBuildings.ItemsSource = SimilarBuildingsOc;
            
            //Disable selection of items on the list
            SimilarBuildings.ItemTapped += (object sender, ItemTappedEventArgs e) => {
                // don't do anything if we just de-selected the row
                if (e.Item == null) return;
                // do something with e.SelectedItem
                ((ListView)sender).SelectedItem = null; // de-select the row
            };

        }

        public void OnRefresh(object sender, EventArgs e)
        {
            //TODO: on refresh maybe check again for buildings nearby
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

            if (position == null)
            {
                return null;
            }

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
                position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }

    }
}