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
            isItAppeared = false;
            Init();
        }

        protected async override void OnAppearing()
        {
            if(!isItAppeared)
            {
                isItAppeared = true;
                try
                {
                    await InitPosizione();
                    struttureListView.ItemsSource = Queries.GetStruttureConsigliate(posizione);
                }
                catch (SqlException)
                {
                    await DisplayAlert("Errore", "Connessione internet assente", "Ok");
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception)
                {
                    await DisplayAlert("Errore", "Impossibile trovare la posizione corrente", "Ok");
                    Process.GetCurrentProcess().Kill();
                }
            }
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
            struttureListView.ItemTemplate = new DataTemplate(typeof(StruttureListItemCell));
            struttureListView.ItemTapped += EventItemTapped;
        }

        private async Task InitPosizione()
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

        private void Init()
        {
            InitStruttureListView();
            infoStruttura = new InfoStruttura();
            Content = struttureListView;
        }

        private ListView struttureListView;
        private Location posizione;
        private InfoStruttura infoStruttura;
        private bool isItAppeared;
    }
}
