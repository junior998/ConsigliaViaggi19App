using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Runtime.InteropServices;

namespace ConsigliaViaggi19App
{
    class MenuLuoghiConsigliati : ContentPage
    {
        public MenuLuoghiConsigliati()
        {
            InitPosizione();
            InitStrutture();
            InitStruttureListView();
            infoStruttura = new InfoStruttura();
            Content = struttureListView;
        }

        private void EventItemTapped(object sender, ItemTappedEventArgs e)
        {
            infoStruttura.Struttura = strutture[e.ItemIndex];
            Navigation.PushAsync(infoStruttura);
            ((ListView)sender).SelectedItem = null;
        }

        private void InitStrutture()
        {
            strutture = new List<Struttura>();
            strutture = Queries.GetStruttureConsigliate(posizione);
        }

        private void InitStruttureListView()
        {
            struttureListView = new ListView();
            struttureListView.RowHeight = 100;
            struttureListView.ItemsSource = strutture;
            struttureListView.ItemTemplate = new DataTemplate(typeof(StruttureConsigliataListItemCell));
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
                await DisplayAlert("Errore", "Errore posizione non disponibile", "Ok");
                Process.GetCurrentProcess().Kill();
            }
        }

        private List<Struttura> strutture;
        private ListView struttureListView;
        private Location posizione;
        private InfoStruttura infoStruttura;
    }
}
