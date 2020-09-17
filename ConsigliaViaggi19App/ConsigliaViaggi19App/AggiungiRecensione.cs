using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class AggiungiRecensione : ContentPage
    {
        public AggiungiRecensione()
        {
            NavigationPage.SetHasBackButton(this, false);
            Title = "Aggiungi recensione";
            InitComponents();
            Content = new StackLayout()
            {
                Children =
                {
                    GetInfoRecensioneFrame(),
                    GetCommentoTestualeFrame(),
                    recensisci
                }
            };
        }

        public int IdStruttura { get; set; }

        protected override void OnAppearing()
        {
            editor.Text = "";
            visibileNicknameCheckBox.IsChecked = false;
            valutazionePicker.SelectedIndex = -1;
        }

        private void InitComponents()
        {
            visibileNicknameCheckBox = new CheckBox()
            {
                IsChecked = false,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            visibileNickanameLabel = new Label()
            {
                Text = "Visibile con il tuo nickname",
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.DarkGreen,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.FillAndExpand

            };
            infoVisibilitaLabel = new Label()
            {
                Text = "Se l'opzione è spuntata gli altri\n" +
                "utenti potranno vedere solo il tuo\n" +
                "nickname",
                TextColor = Color.DarkGreen,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            valutazioneLabel = new Label()
            {
                Text = "Valutazione: ",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.Center
            };
            valutazionePicker = new Picker()
            {
                ItemsSource = new int[] { 1, 2, 3, 4, 5 },
                Title = "Stelle"
            };
            editor = new Editor()
            {
                TextColor = Color.DarkGreen,
                Placeholder = "Commento testuale",
                MaxLength = 200,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            recensisci = new Button()
            { 
                Text = "Recensisci",
                TextColor = Color.White,
                BackgroundColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button))
            };
            recensisci.Clicked += EventRecensisciClicked;
        }

        private async void EventRecensisciClicked(object sender, EventArgs e)
        {
            if (!CheckCorrettezzaRecensione())
                await DisplayAlert("Errore", $"Inserire valutazione e commento testuale almeno lungo {LUNGHEZZA_MINIMA_COMMENTO} caratteri", "Ok");
            else
            {
                bool risposta = await DisplayAlert("Attenzione", "Sei sicuro di voler richiedere la pubblicazione della recensione?", "si", "no");
                try
                {
                    if (risposta)
                    {
                        Recensione recensione = new Recensione
                        {
                            NicknameUtente = UtilityUtente.Nickname,
                            Valutazione = (int)valutazionePicker.SelectedItem,
                            Commento = editor.Text,
                            VisibileConNickname = visibileNicknameCheckBox.IsChecked,
                            DataCreazione = DateTime.Now,
                            IdStruttura = this.IdStruttura
                        };
                        Queries.CaricaRecensione(recensione);
                        await DisplayAlert("Richiesta inviata", "Richiesta di pubblicazione della recensione inviata con successo", "ok");
                        await Navigation.PopAsync();
                    }
                }
                catch (SqlException)
                {
                    await DisplayAlert("Errore", "Connessione internet assente", "Ok");
                }
            }
        }

        private bool CheckCorrettezzaRecensione()
        {
            if (valutazionePicker.SelectedIndex == -1 || editor.Text.Length < LUNGHEZZA_MINIMA_COMMENTO)
                return false;
            return true;
        }

        private StackLayout GetInfoNicknameLayout()
        {
            StackLayout figlio = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    visibileNicknameCheckBox,
                    visibileNickanameLabel
                }
            };
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    figlio,
                    infoVisibilitaLabel
                }
            };
        }

        private StackLayout GetValutazioneLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    valutazioneLabel,
                    valutazionePicker
                }
            };
        }

        private Frame GetInfoRecensioneFrame()
        {
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        GetInfoNicknameLayout(),
                        GetValutazioneLayout()
                    }
                }
            };
        }

        private Frame GetCommentoTestualeFrame()
        {
            return new Frame()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BorderColor = Color.DarkGreen,
                Content = editor
            };
        }

        private CheckBox visibileNicknameCheckBox;
        private Label visibileNickanameLabel;
        private Label infoVisibilitaLabel;
        private Label valutazioneLabel;
        private Picker valutazionePicker;
        private Editor editor;
        private Button recensisci;
        private const int LUNGHEZZA_MINIMA_COMMENTO = 3;
    }
}
