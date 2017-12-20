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
        public Info()
        {
            InitializeComponent();
            SimilarBuildings.ItemsSource = SimilarBuildingsOC;
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio1" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio2" });
            SimilarBuildingsOC.Add(new Building() { Name = "Ktirio3" });
        }
    }
}