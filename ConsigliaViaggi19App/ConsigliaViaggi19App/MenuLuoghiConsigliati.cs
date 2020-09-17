using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Runtime.InteropServices;
using System.Data.SqlClient;

namespace ConsigliaViaggi19App
{
    class MenuLuoghiConsigliati : ContentPage
    {
        public MenuLuoghiConsigliati()
        {
            InitPosizione();
            InitStruttureListView();
            infoStruttura = new InfoStruttura();
            Content = struttureListView;
        }

        private void EventItemTapped(object sender, ItemTappedEventArgs e)
        {
            infoStruttura.Struttura = ((List<Struttura>)struttureListView.ItemsSource)[e.ItemIndex];
            Navigation.PushAsync(infoStruttura);
            ((ListView)sender).SelectedItem = null;
        }

        private void InitStruttureListView()
        {
            struttureListView = new ListView();
            struttureListView.RowHeight = 100;
            try
            {
                struttureListView.ItemsSource = Queries.GetStruttureConsigliate(posizione);
            }
            catch (SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
                Process.GetCurrentProcess().Kill();
            }
            struttureListView.ItemTemplate = new DataTemplate(typeof(StruttureListItemCell));
            struttureListView.ItemTapped += EventItemTapped;
        }

        private async void InitPosizione()
        {
            try
            {
                posizione = await Geolocation.GetLastKnownLocationAsync();
                if(posizione is null)
                {
                    posizione = await Geolocation.GetLocationAsync(new GeolocationRequest()
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Errore", "Impossibile trovare la posizione corrente", "Ok");
                Process.GetCurrentProcess().Kill();
            }
        }

        private ListView struttureListView;
        private Location posizione;
        private InfoStruttura infoStruttura;
    }
}
