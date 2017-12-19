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
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new Scanner());
        }
    }
}
