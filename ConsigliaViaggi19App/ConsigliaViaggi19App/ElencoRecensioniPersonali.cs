using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class ElencoRecensioniPersonali : ContentPage
    {
        public ElencoRecensioniPersonali()
        {
            NavigationPage.SetHasBackButton(this, false);
            Title = "Recensioni personali";
            visualizzaRecensionePersonale = new VisualizzaRecensionePersonale();
            fromRecensionePersonale = false;
            InitListView();
            Content = listView;
        }
        protected async override void OnAppearing()
        {
            try
            {
                if (!fromRecensionePersonale)
                {
                    listView.ItemsSource = null;
                    listView.ItemsSource = Queries.GetRecensioniPersonali(UtilityUtente.Nickname);
                }
                else
                    fromRecensionePersonale = false;
            }
            catch (SqlException)
            {
                await DisplayAlert("Errore", "Connessione internet assente", "Ok");
                await Navigation.PopAsync();
            }
        }
        private void InitListView()
        {
            listView = new ListView();
            listView.ItemTemplate = new DataTemplate(typeof(RecensioniPersonaliListItemCell));
            listView.RowHeight = 100;
            listView.ItemTapped += EventListViewItemTapped;
        }
        private void EventListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            fromRecensionePersonale = true;
            visualizzaRecensionePersonale.RecensionePersonale = (RecensionePersonale)listView.SelectedItem;
            Navigation.PushAsync(visualizzaRecensionePersonale);
            listView.SelectedItem = null;
        }

        private ListView listView;
        private VisualizzaRecensionePersonale visualizzaRecensionePersonale;
        private bool fromRecensionePersonale;
    }
}
