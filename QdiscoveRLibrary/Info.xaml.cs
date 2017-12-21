using System;
using System.Collections.ObjectModel;
using System.Linq;
using QdiscoveR.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        Collection<Building> SimilarBuildingsOC = new ObservableCollection<Building>();
        public Info(string buildingId)
        {
            InitializeComponent();

            //Using this way beacause constructor of a page cannot be async (only in C# 7.1)
            Device.BeginInvokeOnMainThread(async () =>
            {
                var buildingTable = App.MobileService.GetTable<Building>();
                var buildingItem = await buildingTable.Where(x => (x.id == buildingId)).ToListAsync();
                var building = buildingItem.FirstOrDefault();


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

            SimilarBuildings.ItemsSource = SimilarBuildingsOC;
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio1" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio2" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio3" });
        }

        public void OnRefresh(object sender, EventArgs e)
        {
            //TODO: on resfresh maybe check again for buildings nearby
        }
    }
}