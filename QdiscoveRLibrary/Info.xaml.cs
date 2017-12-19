using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QdiscoveR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Info : ContentPage
    {
        ObservableCollection<BuildingInfo> SimilarBuildingsOC = new ObservableCollection<BuildingInfo>();
        public Info()
        {
            InitializeComponent();
            SimilarBuildings.ItemsSource = SimilarBuildingsOC;
            SimilarBuildingsOC.Add(new BuildingInfo { Name = "Ktirio1" });
            SimilarBuildingsOC.Add(new BuildingInfo { Name = "Ktirio2" });
            SimilarBuildingsOC.Add(new BuildingInfo { Name = "Ktirio3" });
        }
    }
}