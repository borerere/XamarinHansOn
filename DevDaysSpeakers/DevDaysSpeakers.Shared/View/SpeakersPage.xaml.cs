﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.ViewModel;


namespace DevDaysSpeakers.View
{
    public partial class SpeakersPage : ContentPage
    {
        SpeakersViewModel vm;
        public SpeakersPage()
        {
            InitializeComponent();

            //Create the view model and set as binding context
            vm = new SpeakersViewModel();
            BindingContext = vm;
            ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;

        }

        private async void ListViewSpeakers_ItemSelected(object sender,SelectedItemChangedEventArgs e)
        {
            var speaker = e.SelectedItem as Speaker;
            if (speaker == null)
                return;

            // await Navigation.PushAsync(new DetailsPage(speaker));
            //ビューモデルを渡す。
            await Navigation.PushAsync(new DetailsPage(speaker, vm));

            ListViewSpeakers.SelectedItem = null;
        } 
       
    }
}
