using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class InfoStruttura : ContentPage
    {
        public InfoStruttura()
        {
            NavigationPage.SetHasBackButton(this, false);
            Title = "Info Struttura";
            InitComponents();
            menuFiltraRecensioni = new MenuFiltraRecensioni();
            menuLogin = new MenuLogin();
            aggiungiRecensione = new AggiungiRecensione();
            premutoRecensici = false;
            StackLayout buttonsLayout = InitButtonsLayout();
            StackLayout labelsLayout = InitLabelsLayout();
            StackLayout mainLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    immagineUno,
                    labelsLayout,
                    buttonsLayout
                }
            };
            ScrollView scrollView = new ScrollView()
            {
                Content = mainLayout
            };
            Content = scrollView;
        }

        public Struttura Struttura
        {
            get
            {
                return struttura;
            }
            set
            {
                struttura = value;
                nomeLabel.Text = struttura.Nome;
                valutazioneMediaLabel.Text = struttura.ValutazioneMediaTesto;
                distanzaLabel.Text = struttura.DistanzaTesto;
                descrizioneLabel.Text = struttura.Descrizione;
                immagineUno.Source = struttura.Immagine;
            }
        }

        protected override void OnAppearing()
        {
            if (premutoRecensici && UtilityUtente.IsUtenteConnesso)
            {
                aggiungiRecensione.IdStruttura = Struttura.Id;
                Navigation.PushAsync(aggiungiRecensione);
                premutoRecensici = false;
            }
        }

        private void InitImages()
        {
            immagineUno = new Image()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }

        private void InitLabels()
        {
            nomeLabel = new Label()
            {
                TextColor = Color.DarkGreen
            };
            valutazioneMediaLabel = new Label()
            {
                TextColor = Color.DarkGreen
            };
            distanzaLabel = new Label()
            {
                TextColor = Color.DarkGreen
            };
            descrizioneLabel = new Label()
            {
                TextColor = Color.DarkGreen
            };
        }

        private void InitButtons()
        {
            recensisciButton = new Button()
            {
                Text = "Recensisci",
                BackgroundColor = Color.DarkGreen,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensisciButton.Clicked += EventRecensisciButtonClicked;
            vediSuMappaButton = new Button()
            {
                Text = "Vedi su mappa",
                BackgroundColor = Color.DarkGreen,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniButton = new Button()
            {
                Text = "Recensioni",
                BackgroundColor = Color.DarkGreen,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniButton.Clicked += EventRecensioniButtonClicked;
        }

        private void EventRecensisciButtonClicked(object sender, EventArgs e)
        {
            premutoRecensici = true;
            if(!UtilityUtente.IsUtenteConnesso)
                Navigation.PushAsync(menuLogin);
            else
            {
                aggiungiRecensione.IdStruttura = Struttura.Id;
                premutoRecensici = false;
                Navigation.PushAsync(aggiungiRecensione);
            }
        }

        private void EventRecensioniButtonClicked(object sender, EventArgs e)
        {
            menuFiltraRecensioni.IdStruttura = struttura.Id;
            Navigation.PushAsync(menuFiltraRecensioni);
        }

        private void InitComponents()
        {
            InitImages();
            InitLabels();
            InitButtons();
        }

        private StackLayout InitButtonsLayout()
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    recensisciButton,
                    vediSuMappaButton,
                    recensioniButton
                }
            };
            return layout;
        }

        private StackLayout InitLabelsLayout()
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    nomeLabel,
                    valutazioneMediaLabel,
                    distanzaLabel
                }
            };
            Frame frameUno = new Frame()
            {
                Content = layout,
                BorderColor = Color.DarkGreen
            };
            Frame frameDue = new Frame()
            {
                Content = descrizioneLabel,
                BorderColor = Color.DarkGreen
            };
            StackLayout layoutFrames = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    frameUno,
                    frameDue
                }
            };
            return layoutFrames;
        }

        private Image immagineUno;
        private Label nomeLabel;
        private Label valutazioneMediaLabel;
        private Label distanzaLabel;
        private Label descrizioneLabel;
        private Button recensisciButton;
        private Button vediSuMappaButton;
        private Button recensioniButton;
        private Struttura struttura;
        private MenuFiltraRecensioni menuFiltraRecensioni;
        private MenuLogin menuLogin;
        private AggiungiRecensione aggiungiRecensione;
        private bool premutoRecensici;
    }
}
