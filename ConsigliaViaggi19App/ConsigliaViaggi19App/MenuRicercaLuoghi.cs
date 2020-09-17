using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace ConsigliaViaggi19App
{
    class MenuRicercaLuoghi : ContentPage
    {
        public MenuRicercaLuoghi()
        {
            InitComponents();
            luoghiTrovati = new LuoghiTrovati();
            Frame mainFrame = GetMainFrame();
            Frame distanzaFrame = GetDistanzaFrame();
            Frame valutazioneFrame = GetValutazioneFrame();
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    mainFrame,
                    distanzaFrame,
                    valutazioneFrame,
                }
            };
            Content = layout;
        }

        private async void EventCercaClicked(object sender, EventArgs e)
        {
            if (!CheckDistanzaEntries() || !CheckValutazioneMediaPickers())
                await DisplayAlert("Errore", "Parametri di ricerca inseriti non corretti", "Ok");
            else
            {
                try
                {
                    Location posizione = await GetCurrentLocation();
                    ParametriRicercaStrutture parametri = GetParametri();
                    parametri.PosizioneCorrente = posizione;
                    luoghiTrovati.Parametri = parametri;
                    await Navigation.PushAsync(luoghiTrovati);
                }
                catch (Exception)
                {
                    await DisplayAlert("Errore", "Impossibile trovare la posizione corrente", "Ok");
                }
            }
        }

        private ParametriRicercaStrutture GetParametri()
        {

            ParametriRicercaStrutture parametri = new ParametriRicercaStrutture()
            {
                NomeStruttura = (nomeStruttura.Text is null) ? "" : nomeStruttura.Text.ToLower(),
                TipoStruttura = tipoStruttura.ItemsSource[tipoStruttura.SelectedIndex].ToString(),
                Citta = citta.ItemsSource[citta.SelectedIndex].ToString(),
                IsFiltroPosizioneAttivo = false
            };
            if (parametri.Citta == "Posizione corrente")
            {
                parametri.IsFiltroPosizioneAttivo = true;
                parametri.DistanzaMinima = double.Parse(distanzaMinimaEntry.Text);
                parametri.DistanzaMassima = double.Parse(distanzaMassimaEntry.Text);
            }
            if (componentiValutazioneAttivi.IsToggled)
            {
                if (valutazioneMediaMinimaPicker.SelectedIndex == 0)
                    parametri.ValutazioneMediaMinima = 0;
                else
                    parametri.ValutazioneMediaMinima = (int)valutazioneMediaMinimaPicker.ItemsSource[valutazioneMediaMinimaPicker.SelectedIndex];
                if (valutazioneMediaMassimaPicker.SelectedIndex == 0)
                    parametri.ValutazioneMediaMassima = 0;
                else
                    parametri.ValutazioneMediaMassima = (int)valutazioneMediaMassimaPicker.ItemsSource[valutazioneMediaMassimaPicker.SelectedIndex];
            }
            return parametri;
        }

        private async Task<Location> GetCurrentLocation()
        {
            Location posizione = await Geolocation.GetLastKnownLocationAsync();
            if (posizione is null)
            {
                posizione = await Geolocation.GetLocationAsync(new GeolocationRequest()
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            return posizione;
        }

        private bool CheckDistanzaEntries()
        {
            if(distanzaMassimaEntry.IsEnabled && distanzaMinimaEntry.IsEnabled)
            {
                if (distanzaMinimaEntry.Text is null || distanzaMassimaEntry.Text is null)
                    return false;
                double distanzaMinima = double.Parse(distanzaMinimaEntry.Text);
                double distanzaMassima = double.Parse(distanzaMassimaEntry.Text);
                if (distanzaMinima < 0 || distanzaMassima < 0 || distanzaMinima > distanzaMassima)
                    return false;
            }
            return true;
        }

        private bool CheckValutazioneMediaPickers()
        {
            if(componentiValutazioneAttivi.IsToggled)
            {
                if (valutazioneMediaMinimaPicker.SelectedIndex == -1 || valutazioneMediaMassimaPicker.SelectedIndex == -1)
                    return false;
                int indexValutazioneMediaMinima = valutazioneMediaMinimaPicker.SelectedIndex;
                int indexValutazioneMediaMassima = valutazioneMediaMassimaPicker.SelectedIndex;
                if (indexValutazioneMediaMinima > indexValutazioneMediaMassima)
                    return false;
            }
            return true;
        }

        private void EventComponentiValutazioneAttiviToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                valutazioneMediaMinimaPicker.IsEnabled = true;
                valutazioneMediaMassimaPicker.IsEnabled = true;
            }
            else
            {
                valutazioneMediaMinimaPicker.IsEnabled = false;
                valutazioneMediaMassimaPicker.IsEnabled = false;
            }
        }

        private void EventCittaSelectedIndexChanged(object sender, EventArgs e)
        {
            string cittaSelezionata = ((Picker)sender).SelectedItem.ToString();
            if(cittaSelezionata == "Posizione corrente")
            {
                distanzaMinimaEntry.IsEnabled = true;
                distanzaMassimaEntry.IsEnabled = true;
            }
            else
            {
                distanzaMinimaEntry.IsEnabled = false;
                distanzaMassimaEntry.IsEnabled = false;
            }
        }

        private void InitComponents()
        {
            InitNomeStruttura();
            InitTipoStruttura();
            InitDistanzaMinimaLabel();
            InitDistanzaMassimaLabel();
            InitDistanzaMinimaEntry();
            InitDistanzaMassimaEntry();
            InitFiltraPerDistanza();
            InitCitta();
            InitValutazioneMediaMinimaLabel();
            InitValutazioneMediaMassimaLabel();
            InitValutazioneMediaMinimaPicker();
            InitValutazioneMediaMassimaPicker();
            InitFiltraPerValutazioneMedia();
            InitComponentiValutazioneAttivi();
            InitCerca();
        }

        private void InitNomeStruttura()
        {
            nomeStruttura = new Entry()
            {
                Keyboard = Keyboard.Default,
                Placeholder = "Nome Struttura",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Color.DarkGreen
            };
        }

        private void InitTipoStruttura()
        {
            tipoStruttura = new Picker()
            {
                Title = "Tipo Struttura",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Color.DarkGreen
            };
            List<string> tipiStrutture = null;
            try
            {
                tipiStrutture = Queries.GetTipiStrutture();
            }
            catch (SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
                Process.GetCurrentProcess().Kill();
            }
            tipiStrutture.Add("Qualsiasi struttura");
            tipoStruttura.ItemsSource = tipiStrutture;
            tipoStruttura.SelectedIndex = tipoStruttura.ItemsSource.Count - 1;
        }

        private void InitCitta()
        {
            citta = new Picker()
            {
                Title = "Città",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Color.DarkGreen
            };
            citta.SelectedIndexChanged += EventCittaSelectedIndexChanged;
            List<string> listaCitta = null;
            try
            {
                listaCitta = Queries.GetCitta();
            }
            catch(SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
                Process.GetCurrentProcess().Kill();
            }
            listaCitta.Add("Posizione corrente");
            citta.ItemsSource = listaCitta;
            citta.SelectedIndex = citta.ItemsSource.Count - 1;
        }

        private void InitDistanzaMinimaLabel()
        {
            distanzaMinimaLabel = new Label()
            {
                Text = "Distanza minima:",
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void InitDistanzaMassimaLabel()
        {
            distanzaMassimaLabel = new Label()
            {
                Text = "Distanza massima:",
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void InitDistanzaMinimaEntry()
        {
            distanzaMinimaEntry = new Entry()
            {
                Placeholder = "km",
                Keyboard = Keyboard.Numeric,
                WidthRequest = 150,
                TextColor = Color.DarkGreen
            };
        }

        private void InitDistanzaMassimaEntry()
        {
            distanzaMassimaEntry = new Entry()
            {
                Placeholder = "km",
                Keyboard = Keyboard.Numeric,
                WidthRequest = 150,
                TextColor = Color.DarkGreen
            };
        }

        private void InitFiltraPerDistanza()
        {
            filtraPerDistanza = new Label()
            {
                Text = "Filtra Per Distanza",
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold
            };
        }

        private void InitCerca()
        {
            cerca = new Button()
            {
                BackgroundColor = Color.DarkGreen,
                TextColor = Color.White,
                Text = "Cerca",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 35,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button))
            };
            cerca.Clicked += EventCercaClicked;
        }

        private void InitValutazioneMediaMinimaLabel()
        {
            valutazioneMediaMinimaLabel = new Label()
            {
                Text = "Valutazione Media minima:",
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void InitValutazioneMediaMassimaLabel()
        {
            valutazioneMediaMassimaLabel = new Label()
            {
                Text = "Valutazione Media massima:",
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }

        private void InitValutazioneMediaMinimaPicker()
        {
            valutazioneMediaMinimaPicker = new Picker()
            {
                Title = "Stelle",
                ItemsSource = new object[] { "Nessuna Valutazione", 1, 2, 3, 4, 5 },
                WidthRequest = 180,
                IsEnabled = false,
                TextColor = Color.DarkGreen
            };
        }

        private void InitValutazioneMediaMassimaPicker()
        {
            valutazioneMediaMassimaPicker = new Picker()
            {
                Title = "Stelle",
                ItemsSource = new object[] { "Nessuna Valutazione", 1, 2, 3, 4, 5 },
                WidthRequest = 180,
                IsEnabled = false,
                TextColor = Color.DarkGreen
            };
        }

        private void InitFiltraPerValutazioneMedia()
        {
            filtraPerValutazioneMedia = new Label()
            {
                Text = "Filtra per valutazione media",
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold
            };
        }

        private void InitComponentiValutazioneAttivi()
        {
            componentiValutazioneAttivi = new Xamarin.Forms.Switch()
            {
                IsToggled = false
            };
            componentiValutazioneAttivi.Toggled += EventComponentiValutazioneAttiviToggled;
        }

        private Frame GetMainFrame()
        {
            StackLayout layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    nomeStruttura,
                    tipoStruttura,
                    citta
                }
            };
            Frame mainFrame = new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = layout
            };
            return mainFrame;
        }

        private StackLayout GetDistanzaLabelsLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    distanzaMinimaLabel,
                    distanzaMassimaLabel
                }
            };
        }

        private StackLayout GetDistanzaEntriesLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    distanzaMinimaEntry,
                    distanzaMassimaEntry
                }
            };
        }

        private Frame GetDistanzaFrame()
        {
            StackLayout distanzaLabelsLayout = GetDistanzaLabelsLayout();
            StackLayout distanzaEntriesLayout = GetDistanzaEntriesLayout();
            StackLayout distanzaLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    distanzaLabelsLayout,
                    distanzaEntriesLayout
                }
            };
            StackLayout mainLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    filtraPerDistanza,
                    distanzaLayout
                }
            };
            return new Frame()
            {
                VerticalOptions = LayoutOptions.Start,
                BorderColor = Color.DarkGreen,
                Content = mainLayout
            }; 
        }

        private StackLayout GetValutazioneLabelsLayout()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    valutazioneMediaMinimaLabel,
                    valutazioneMediaMassimaLabel
                }
            };
        }

        private StackLayout GetValutazioneMediaPickers()
        {
            return new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    valutazioneMediaMinimaPicker,
                    valutazioneMediaMassimaPicker
                }
            };
        }

        private Frame GetValutazioneFrame()
        {
            StackLayout layoutUno = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    filtraPerValutazioneMedia,
                    componentiValutazioneAttivi
                }
            };
            StackLayout layoutDue = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    GetValutazioneLabelsLayout(),
                    GetValutazioneMediaPickers(),
                }
            };
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        layoutUno,
                        layoutDue,
                        cerca
                    }
                }
            };
        }

        private Entry nomeStruttura;
        private Picker tipoStruttura;
        private Picker citta;
        private Label filtraPerDistanza;
        private Label distanzaMinimaLabel;
        private Entry distanzaMinimaEntry;
        private Label distanzaMassimaLabel;
        private Entry distanzaMassimaEntry;
        private Label filtraPerValutazioneMedia;
        private Xamarin.Forms.Switch componentiValutazioneAttivi;
        private Label valutazioneMediaMinimaLabel;
        private Picker valutazioneMediaMinimaPicker;
        private Label valutazioneMediaMassimaLabel;
        private Picker valutazioneMediaMassimaPicker;
        private LuoghiTrovati luoghiTrovati;
        private Button cerca;
    }
}
