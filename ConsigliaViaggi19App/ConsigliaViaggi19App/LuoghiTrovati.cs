using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class LuoghiTrovati : ContentPage
    {
        public LuoghiTrovati()
        {
            infoStruttura = new InfoStruttura();
            InitListView();
            Content = listView;
            Title = "Luoghi Trovati";
            NavigationPage.SetHasBackButton(this, false);
        }

        private void EventItemTapped(object sender, ItemTappedEventArgs e)
        {
            List<Struttura> strutture = (List<Struttura>)listView.ItemsSource;
            infoStruttura.Struttura = strutture[e.ItemIndex];
            Navigation.PushAsync(infoStruttura);
            ((ListView)sender).SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            try
            {
                List<Struttura> strutture = Queries.GetLuoghiTrovati(Parametri);
                if(strutture.Count != 0)
                    listView.ItemsSource = strutture;
                else
                {
                    DisplayAlert("Errore", "Nessuna struttura trovata", "Ok");
                    Navigation.PopAsync();
                }
            }
            catch(SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
            }
        }

        private void InitListView()
        {
            listView = new ListView();
            listView.RowHeight = 100;
            listView.ItemTemplate = new DataTemplate(typeof(StruttureListItemCell));
            listView.ItemTapped += EventItemTapped;
        }

        public ParametriRicercaStrutture Parametri { get; set; }
        private ListView listView;
        private InfoStruttura infoStruttura;
    }
}
