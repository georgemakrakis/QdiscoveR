using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QdiscoveR.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        ObservableCollection<Building> SimilarBuildingsOC = new ObservableCollection<Building>();
        public Info(String BuildingID)
        {
            InitializeComponent();
            //TODO: Connect to database
            //send BuildingID
            //receive Name,Lng,Lat,Info
            Building B = new Building();//from server

            BName.Text = B.Name;
            BInfo.Text = B.Info;
            BLocation.Text = B.Lat + " " + B.Lng;
            SimilarBuildings.ItemsSource = SimilarBuildingsOC;
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio1" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio2" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio3" });
        }
    }
}