using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class VisualizzaRecensionePersonale : ContentPage
    {
        public VisualizzaRecensionePersonale()
        {
            NavigationPage.SetHasBackButton(this, false);
            Title = "Visualizza recensione personale";
            infoStruttura = new InfoStruttura();
            InitComponents();
            Content = new ScrollView()
            { 
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        GetUpperFrame(),
                        GetMiddleFrame(),
                        GetLowerFrame()
                    }
                }
            };
        }
        public RecensionePersonale RecensionePersonale { get; set; }
        protected override void OnAppearing()
        {
            nomeStruttura.Text = RecensionePersonale.NomeStruttura;
            immagine.Source = RecensionePersonale.ImmagineStruttura;
            statoRecensioneEntry.Text = RecensionePersonale.Stato;
            dataPubblicazioneEntry.Text = RecensionePersonale.Recensione.DataCreazione.ToString("dd/MM/yyyy");
            valutazioneEntry.Text = RecensionePersonale.Recensione.Valutazione.ToString();
            visibileConNicknameCheckBox.IsChecked = RecensionePersonale.Recensione.VisibileConNickname;
            commentoEditor.Text = RecensionePersonale.Recensione.Commento;
        }
        private void InitComponents()
        {
            InitImages();
            InitLabels();
            InitEntries();
            InitEditors();
            InitButtons();
            InitCheckBoxes();
        }
        private void InitImages()
        {
            immagine = new Image()
            { 
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
        }
        private void InitLabels()
        {
            nomeStruttura = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };
            statoRecensioneLabel = new Label()
            {
                Text = "Stato recensione: ",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            dataPubblicazioneLabel = new Label()
            {
                Text = "Data pubblicazione: ",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            valutazioneLabel = new Label()
            {
                Text = "Valutazione: ",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            visibileConNicknameLabel = new Label()
            { 
                Text = "Visibile con nickname: ",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            commentoLabel = new Label()
            {
                Text = "Commento",
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
        }
        private void InitEntries()
        {
            statoRecensioneEntry = new Entry()
            {
                TextColor = Color.DarkGreen,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            dataPubblicazioneEntry = new Entry()
            {
                TextColor = Color.DarkGreen,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            valutazioneEntry = new Entry()
            {
                TextColor = Color.DarkGreen,
                IsReadOnly = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }
        private void InitCheckBoxes()
        {
            visibileConNicknameCheckBox = new CheckBox()
            {
                HorizontalOptions = LayoutOptions.Center,
                IsEnabled = false
            };
        }
        private void InitEditors()
        {
            commentoEditor = new Editor()
            {
                IsReadOnly = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Color.Green,
            };
        }
        private void InitButtons()
        {
            visualizzaStruttura = new Button()
            {
                Text = "Visualizza struttura",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                BackgroundColor = Color.DarkGreen,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button))
            };
            visualizzaStruttura.Clicked += EventVisualizzaStrutturaClicked;
        }
        private void EventVisualizzaStrutturaClicked(object sender, EventArgs e)
        {
            try
            {
                infoStruttura.Struttura = Queries.GetStruttura(RecensionePersonale.IdStruttura);
                Navigation.PushAsync(infoStruttura);
            }
            catch(SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
            }
        }
        private Frame GetUpperFrame()
        {
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        immagine,
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Vertical,
                            Children =
                            {
                                nomeStruttura,
                                visualizzaStruttura
                            }
                        }
                    }
                }
            };
        }
        private Frame GetMiddleFrame()
        {
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                statoRecensioneLabel,
                                statoRecensioneEntry
                            }
                        },
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                dataPubblicazioneLabel,
                                dataPubblicazioneEntry
                            }
                        },
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                valutazioneLabel,
                                valutazioneEntry
                            }
                        },
                        new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                visibileConNicknameLabel,
                                visibileConNicknameCheckBox
                            }
                        }
                    }
                }
            };
        }
        private Frame GetLowerFrame()
        {
            return new Frame()
            {
                BorderColor = Color.DarkGreen,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {
                        commentoLabel, 
                        commentoEditor
                    }
                }
            };
        }
        
        private Image immagine;
        private Label nomeStruttura;
        private Button visualizzaStruttura;
        private Label statoRecensioneLabel;
        private Label dataPubblicazioneLabel;
        private Label valutazioneLabel;
        private Label visibileConNicknameLabel;
        private Entry statoRecensioneEntry;
        private Entry dataPubblicazioneEntry;
        private Entry valutazioneEntry;
        private CheckBox visibileConNicknameCheckBox;
        private Editor commentoEditor;
        private Label commentoLabel;
        private InfoStruttura infoStruttura;
    }
}
