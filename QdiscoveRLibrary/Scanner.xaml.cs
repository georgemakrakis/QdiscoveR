using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QdiscoveR
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Scanner : ContentPage
	{
		public Scanner ()
		{
			InitializeComponent ();
		}
        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Info(),true);
        }
    }
}