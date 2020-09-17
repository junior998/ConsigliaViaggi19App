using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class VisualizzaRecensione : ContentPage
    {
        public VisualizzaRecensione()
        {
            Title = "Visualizza recensione";
            NavigationPage.SetHasBackButton(this, false);
            InitComponets();
            Content = new ScrollView()
            {
                Content = new StackLayout()
                {
                    Children =
                    {
                        GetInfoRecensioneFrame(),
                        GetCommentoLabelsFrame()
                    }
                }
            };
        }

        public Recensione Recensione { get; set; }

        protected override void OnAppearing()
        {
            nomeAccountLabel.Text = $"{Recensione.IdentificativoUtente}";
            valutazioneStrutturaLabel.Text = $"{Recensione.ValutazioneText}";
            dataPubblicazioneLabel.Text = $"Data pubblicazione: {Recensione.DataCreazione.ToString("dd/MM/yyyy")}";
            commentoUtenteLabel.Text = Recensione.Commento;
        }

        private void InitComponets()
        {
            InitInfoRecensioneLabels();
            InitInfoCommentoLabels();
        }

        private void InitInfoRecensioneLabels()
        {
            nomeAccountLabel = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            valutazioneStrutturaLabel = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            dataPubblicazioneLabel = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }

        private void InitInfoCommentoLabels()
        {
            commentoLabel = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "Commento",
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            commentoUtenteLabel = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };
        }

        private Frame GetInfoRecensioneFrame()
        {
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        nomeAccountLabel,
                        valutazioneStrutturaLabel,
                        dataPubblicazioneLabel
                    }
                }
            };
        }

        private Frame GetCommentoLabelsFrame()
        {
            return new Frame()
            {
                BorderColor = Color.Green,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        commentoLabel,
                        commentoUtenteLabel
                    }
                }
            };
        }

        private Label nomeAccountLabel;
        private Label valutazioneStrutturaLabel;
        private Label dataPubblicazioneLabel;
        private Label commentoLabel;
        private Label commentoUtenteLabel;
    }
}
