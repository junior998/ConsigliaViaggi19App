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
using System.Threading.Tasks;

namespace ConsigliaViaggi19App
{
    class MenuLuoghiConsigliati : ContentPage
    {
        public MenuLuoghiConsigliati()
        {
            Init();
        }

        private void EventItemTapped(object sender, ItemTappedEventArgs e)
        {
            infoStruttura.Struttura = (Struttura)struttureListView.SelectedItem;
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

        private async Task InitPosizione()
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

        private async void Init()
        {
            await InitPosizione();
            InitStruttureListView();
            infoStruttura = new InfoStruttura();
            Content = struttureListView;
        }

        private ListView struttureListView;
        private Location posizione;
        private InfoStruttura infoStruttura;
    }
}
