using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class RecensioniTrovate : ContentPage
    {
        public RecensioniTrovate()
        {
            Title = "Recensioni trovate";
            NavigationPage.SetHasBackButton(this, false);
            InitListView();
            visualizzaRecensione = new VisualizzaRecensione();
            Content = listView;
        }

        public ParametriRicercaRecensione Parametri { get; set; }

        protected override void OnAppearing()
        {
            try
            {
                List<Recensione> recensioni = Queries.GetRecensioni(Parametri);
                if (recensioni.Count != 0)
                    listView.ItemsSource = recensioni;
                else
                {
                    DisplayAlert("Errore", "Nessuna recensione soddisfa i parametri di ricerca", "Ok");
                    Navigation.PopAsync();
                }
            }
            catch (SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
                Navigation.PopAsync();
            }
        }

        private void InitListView()
        {
            listView = new ListView();
            listView.ItemTemplate = new DataTemplate(typeof(RecensioniListITemCell));
            listView.RowHeight = 90;
            listView.ItemTapped += EventListViewItemTapped;
        }

        private void EventListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            visualizzaRecensione.Recensione = ((List<Recensione>)listView.ItemsSource)[e.ItemIndex];
            Navigation.PushAsync(visualizzaRecensione);
            ((ListView)sender).SelectedItem = null;
        }

        private ListView listView;
        private VisualizzaRecensione visualizzaRecensione;
    }
}
