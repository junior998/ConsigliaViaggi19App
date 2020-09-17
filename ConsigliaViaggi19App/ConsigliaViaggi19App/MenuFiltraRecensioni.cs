using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class MenuFiltraRecensioni : ContentPage
    {
        public MenuFiltraRecensioni()
        {
            Title = "Cerca recensioni";
            NavigationPage.SetHasBackButton(this, false);
            recensioniTrovate = new RecensioniTrovate();
            InitComponents();
            fromRecensioniTrovate = false;
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    GetRiepilogoRecensioniFrame(),
                    GetValutazioniFrame(),
                    GetFiltroDataFrame(),
                    cercaButton
                }
            };
            Content = layout;
        }

        public int IdStruttura { get; set; }

        protected async override void OnAppearing()
        {
            if (!fromRecensioniTrovate)
            {
                try
                {
                    AzzeraQuantitaRecensioniLabels();
                    AzzeraQuantitaRecensioniProgressBars();
                    List<Tuple<int, int>> quantitaRecensioni = Queries.GetQuantitativoRecensioni(IdStruttura);
                    SetValueQuantitaRecensioniLabels(quantitaRecensioni);
                    SetValueRecensioniStelleProgressBars(quantitaRecensioni);
                    fromRecensioniTrovate = false;
                }
                catch (SqlException)
                {
                    await DisplayAlert("Errore", "Connessione internet assente", "Ok");
                    await Navigation.PopAsync();
                }
            }
            else
                fromRecensioniTrovate = false;
        }

        private void SetValueQuantitaRecensioniLabels(List<Tuple<int, int>> quantitaRecensioni)
        {
            foreach (Tuple<int, int> tuple in quantitaRecensioni)
                switch (tuple.Item1)
                {
                    case 1:
                        quantitaRecensioniUnaStella.Text = tuple.Item2.ToString();
                        break;
                    case 2:
                        quantitaRecensioniDueStelle.Text = tuple.Item2.ToString();
                        break;
                    case 3:
                        quantitaRecensioniTreStelle.Text = tuple.Item2.ToString();
                        break;
                    case 4:
                        quantitaRecensioniQuattroStelle.Text = tuple.Item2.ToString();
                        break;
                    case 5:
                        quantitaRecensioniCinqueStelle.Text = tuple.Item2.ToString();
                        break;
                }
        }

        private void SetValueRecensioniStelleProgressBars(List<Tuple<int, int>> quantitaRecensioni)
        {
            int totale = 0;
            foreach (Tuple<int, int> tuple in quantitaRecensioni)
                totale += tuple.Item2;
            foreach (Tuple<int, int> tuple in quantitaRecensioni)
            {
                double percentuale = (tuple.Item2 * 100) / totale;
                switch (tuple.Item1)
                {
                    case 1:
                        recensioniUnaStellaProgressBar.Progress = percentuale / 100;
                        break;
                    case 2:
                        recensioniDueStelleProgressBar.Progress = percentuale / 100;
                        break;
                    case 3:
                        recensioniTreStelleProgressBar.Progress = percentuale / 100;
                        break;
                    case 4:
                        recensioniQuattroStelleProgressBar.Progress = percentuale / 100;
                        break;
                    case 5:
                        recensioniCinqueStelleProgressBar.Progress = percentuale / 100;
                        break;
                }
            }
        }

        private void InitComponents()
        {
            InitRecensioniStelleLabels();
            InitRecensioniStelleProgressBars();
            InitQuantitaRecensioniLabels();
            InitValutazioniLabels();
            InitValutazioniPickers();
            InitDataLabels();
            InitDataPickers();
            InitCercaButton();
        }

        private void InitRecensioniStelleLabels()
        {
            recensioniUnaStellaLabel = new Label()
            {
                Text = "Recensioni 1 Stella",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniDueStelleLabel = new Label()
            {
                Text = "Recensioni 2 Stelle",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniTreStelleLabel = new Label()
            {
                Text = "Recensioni 3 Stelle",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniQuattroStelleLabel = new Label()
            {
                Text = "Recensioni 4 Stelle",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            recensioniCinqueStelleLabel = new Label()
            {
                Text = "Recensioni 5 Stelle",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }

        private void InitRecensioniStelleProgressBars()
        {
            recensioniUnaStellaProgressBar = new ProgressBar()
            {
                ProgressColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150
            };
            recensioniDueStelleProgressBar = new ProgressBar()
            {
                ProgressColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150
            };
            recensioniTreStelleProgressBar = new ProgressBar()
            {
                ProgressColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150
            };
            recensioniQuattroStelleProgressBar = new ProgressBar()
            {
                ProgressColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150
            };
            recensioniCinqueStelleProgressBar = new ProgressBar()
            {
                ProgressColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 150
            };
        }

        private void InitQuantitaRecensioniLabels()
        {
            quantitaRecensioniUnaStella = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "0",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            quantitaRecensioniDueStelle = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "0",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            quantitaRecensioniTreStelle = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "0",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            quantitaRecensioniQuattroStelle = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "0",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            quantitaRecensioniCinqueStelle = new Label()
            {
                TextColor = Color.DarkGreen,
                Text = "0",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }

        private StackLayout GetRecensioniLabelsLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    recensioniCinqueStelleLabel,
                    recensioniQuattroStelleLabel,
                    recensioniTreStelleLabel,
                    recensioniDueStelleLabel,
                    recensioniUnaStellaLabel
                }
            };
        }

        private StackLayout GetRecensioniProgressBarsLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    recensioniCinqueStelleProgressBar,
                    recensioniQuattroStelleProgressBar,
                    recensioniTreStelleProgressBar,
                    recensioniDueStelleProgressBar,
                    recensioniUnaStellaProgressBar
                }
            };
        }

        private StackLayout GetQuantitaRecensioniLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    quantitaRecensioniCinqueStelle,
                    quantitaRecensioniQuattroStelle,
                    quantitaRecensioniTreStelle,
                    quantitaRecensioniDueStelle,
                    quantitaRecensioniUnaStella
                }
            };
        }

        private Frame GetRiepilogoRecensioniFrame()
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    GetRecensioniLabelsLayout(),
                    GetRecensioniProgressBarsLayout(),
                    GetQuantitaRecensioniLayout()
                }
            };
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = layout
            };
        }

        private void InitValutazioniLabels()
        {
            filtroValutazioneLabel = new Label()
            {
                Text = "Filtro valutazioni",
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold,
            };
            valutazioneMinimaLabel = new Label()
            {
                Text = "Minima:",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            valutazioneMassimaLabel = new Label()
            {
                Text = "Massima: ",
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void InitValutazioniPickers()
        {
            valutazioneMinimaPicker = new Picker()
            {
                Title = "Stelle",
                ItemsSource = new int[] { 1, 2, 3, 4, 5 },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 100,
                SelectedIndex = 0

            };
            valutazioneMassimaPicker = new Picker()
            {
                Title = "Stelle",
                ItemsSource = new int[] { 1, 2, 3, 4, 5 },
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 100,
                SelectedIndex = 5
            };
        }

        private StackLayout GetValutazioniLabelsLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    valutazioneMinimaLabel,
                    valutazioneMassimaLabel
                }
            };
        }

        private StackLayout GetValutazioniPickersLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    valutazioneMinimaPicker,
                    valutazioneMassimaPicker
                }
            };
        }

        private Frame GetValutazioniFrame()
        {
            StackLayout layoutFiglio = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    GetValutazioniLabelsLayout(),
                    GetValutazioniPickersLayout()
                }
            };
            StackLayout layoutPadre = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    filtroValutazioneLabel,
                    layoutFiglio
                }
            };
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = layoutPadre
            };
        }

        private void InitDataLabels()
        {
            filtroDataLabel = new Label()
            {
                Text = "Filtro valutazioni",
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold
            };
            dataDalLabel = new Label()
            {
                Text = "Data dal: ",
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            dataAlLabel = new Label()
            {
                Text = "Data al: ",
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
        }

        private void InitDataPickers()
        {
            dataDalDatePicker = new DatePicker()
            {
                MaximumDate = DateTime.Today,
                WidthRequest = 150,
                Format = "dd/MM/yyyy"
            };
            dataAlDatePicker = new DatePicker()
            {
                MaximumDate = DateTime.Today,
                WidthRequest = 150,
                Format = "dd/MM/yyyy"
            };
        }

        private StackLayout GetDataLayoutLabels()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    dataDalLabel,
                    dataAlLabel
                }
            };
        }

        private StackLayout GetDataLayoutDatePickers()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    dataDalDatePicker,
                    dataAlDatePicker
                }
            };
        }

        private Frame GetFiltroDataFrame()
        {
            StackLayout layoutFiglio = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    GetDataLayoutLabels(),
                    GetDataLayoutDatePickers()
                }
            };
            StackLayout layoutPadre = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    filtroDataLabel,
                    layoutFiglio
                }
            };
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = layoutPadre
            };
        }

        private void InitCercaButton()
        {
            cercaButton = new Button()
            {
                Text = "Cerca",
                TextColor = Color.White,
                BackgroundColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button))
            };
            cercaButton.Clicked += EventCercaButtonClicked;
        }

        private bool CheckValutazioniPickers()
        {
            if (valutazioneMassimaPicker.SelectedIndex < valutazioneMinimaPicker.SelectedIndex || valutazioneMassimaPicker.SelectedIndex == -1
                || valutazioneMinimaPicker.SelectedIndex == -1)
                return false;
            return true;
        }

        private bool CheckDatePickers()
        {
            if (DateTime.Compare(dataDalDatePicker.Date, dataAlDatePicker.Date) > 0)
                return false;
            return true;
        }

        private void EventCercaButtonClicked(object sender, EventArgs e)
        {
            if  (!CheckDatePickers() || !CheckValutazioniPickers())
                DisplayAlert("Errore", "Parametri inseriti non corretti", "Ok");
            else
            {
                fromRecensioniTrovate = true;
                recensioniTrovate.Parametri = GetParametri();
                Navigation.PushAsync(recensioniTrovate);
            }
        }

        private void AzzeraQuantitaRecensioniLabels()
        {
            quantitaRecensioniUnaStella.Text = "0";
            quantitaRecensioniDueStelle.Text = "0";
            quantitaRecensioniTreStelle.Text = "0";
            quantitaRecensioniQuattroStelle.Text = "0";
            quantitaRecensioniCinqueStelle.Text = "0";
        }

        private void AzzeraQuantitaRecensioniProgressBars()
        {
            recensioniUnaStellaProgressBar.Progress = 0;
            recensioniDueStelleProgressBar.Progress = 0;
            recensioniTreStelleProgressBar.Progress = 0;
            recensioniQuattroStelleProgressBar.Progress = 0;
            recensioniCinqueStelleProgressBar.Progress = 0;
        }

        private ParametriRicercaRecensione GetParametri()
        {
            return new ParametriRicercaRecensione()
            {
                ValutazioneMinimo = (int)valutazioneMinimaPicker.ItemsSource[valutazioneMinimaPicker.SelectedIndex],
                ValutazioneMassimo = (int)valutazioneMassimaPicker.ItemsSource[valutazioneMassimaPicker.SelectedIndex],
                IdStruttura = this.IdStruttura,
                DataAl = dataAlDatePicker.Date,
                DataDal = dataDalDatePicker.Date
            };
        }

        private Label recensioniUnaStellaLabel;
        private Label recensioniDueStelleLabel;
        private Label recensioniTreStelleLabel;
        private Label recensioniQuattroStelleLabel;
        private Label recensioniCinqueStelleLabel;
        private ProgressBar recensioniUnaStellaProgressBar;
        private ProgressBar recensioniDueStelleProgressBar;
        private ProgressBar recensioniTreStelleProgressBar;
        private ProgressBar recensioniQuattroStelleProgressBar;
        private ProgressBar recensioniCinqueStelleProgressBar;
        private Label quantitaRecensioniUnaStella;
        private Label quantitaRecensioniDueStelle;
        private Label quantitaRecensioniTreStelle;
        private Label quantitaRecensioniQuattroStelle;
        private Label quantitaRecensioniCinqueStelle;
        private Label filtroValutazioneLabel;
        private Label valutazioneMinimaLabel;
        private Label valutazioneMassimaLabel;
        private Picker valutazioneMinimaPicker;
        private Picker valutazioneMassimaPicker;
        private Label filtroDataLabel;
        private Label dataDalLabel;
        private Label dataAlLabel;
        private DatePicker dataDalDatePicker;
        private DatePicker dataAlDatePicker;
        private Button cercaButton;
        private RecensioniTrovate recensioniTrovate;
        private bool fromRecensioniTrovate;
    }
}
