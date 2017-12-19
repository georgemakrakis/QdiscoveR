﻿using System;
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
            //Remove the bar from the first page
            NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Scanner(), true);
        }
    }
}
