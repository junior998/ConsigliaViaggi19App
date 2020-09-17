using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class MenuAccount : ContentPage
    {
        public MenuAccount()
        {
            primoAccesso = false;
            InitMenuLogin();
            InitNicknameLabel();
            InitNomeLabel();
            InitCognomeLabel();
            InitLuoghiRecensiti();
            InitIscrittoDal();
            InitLogoutButton();
            InitRecensioniPersonaliButton();
            InitEffettuareAccessoLabel();
            InitLoginButton();
            Frame frameLabels = InitFrameLabels();
            Frame frameButtons = InitFrameButtons();
            stackLayoutAccessoEffettuato = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    frameLabels,
                    frameButtons
                }
            };
            Frame frameEffettuareAccesso = InitFrameEffettuareAccesso();
            stackLayoutEffettuareAccesso = new StackLayout
            {
                Children = { frameEffettuareAccesso }
            };
            Title = "Menu Account";
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            if (UtilityUtente.IsUtenteConnesso && primoAccesso == false)
            {
                nicknameLabel.Text = $"Nickname: {UtilityUtente.Nickname}";
                try
                {
                    DataTable dettagliUtenteTable = Queries.GetDettagliUtente(UtilityUtente.Nickname);
                    DataTable luoghiRecensitiTable = Queries.GetLuoghiRecensiti(UtilityUtente.Nickname);
                    CaricaDatiUtente(dettagliUtenteTable);
                    CaricaLuoghiRecensitiUtente(luoghiRecensitiTable);
                    Content = stackLayoutAccessoEffettuato;
                    primoAccesso = true;
                }
                catch (SqlException)
                {
                    DisplayAlert("Errore", "Connessione internet assente", "Ok");
                    Content = stackLayoutEffettuareAccesso;
                    UtilityUtente.IsUtenteConnesso = false;
                }
            }
            else if (UtilityUtente.IsUtenteConnesso && primoAccesso == true)
                Content = stackLayoutAccessoEffettuato;
            else
                Content = stackLayoutEffettuareAccesso;
        }
        
        private void EventClickedLogoutButton(object sender, EventArgs args)
        {
            UtilityUtente.IsUtenteConnesso = false;
            Content = stackLayoutEffettuareAccesso;
            primoAccesso = false;
        }

        private void EventClickedLoginButton(object sendet, EventArgs args)
        {
            Navigation.PushAsync(menuLogin);
        }

        private void CaricaDatiUtente(DataTable dettagliUtenteTable)
        {
            foreach (DataRow riga in dettagliUtenteTable.Rows)
            {
                nomeLabel.Text = $"Nome: {riga["nome"]}";
                cognomeLabel.Text = $"Cognome: {riga["cognome"]}";
                iscrittoDalLabel.Text = $"Iscritto dal: {((DateTime)riga["dataIscrizione"]).ToString("dd/MM/yyyy")}";
            }
        }

        private void CaricaLuoghiRecensitiUtente(DataTable luoghiRecensitiTable)
        {
            foreach (DataRow riga in luoghiRecensitiTable.Rows)
                luoghiRecensitiLabel.Text = $"Luoghi Recensiti {riga["luoghiRecensiti"]}";
        }

        private void InitNicknameLabel()
        {
            nicknameLabel = new Label();
        }

        private void InitNomeLabel()
        {
            nomeLabel = new Label();
        }

        private void InitCognomeLabel()
        {
            cognomeLabel = new Label();
        }

        private void InitLuoghiRecensiti()
        {
            luoghiRecensitiLabel = new Label();
        }

        private void InitIscrittoDal()
        {
            iscrittoDalLabel = new Label();
        }

        private void InitRecensioniPersonaliButton()
        {
            recensioniPersonaliButton = new Button
            {
                Text = "Recensioni personali",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                BackgroundColor = Color.Green,
                TextColor = Color.White
            };
        }

        private void InitLogoutButton()
        {
            logoutButton = new Button
            {
                Text = "Logout",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                BackgroundColor = Color.Green,
                TextColor = Color.White
            };
            logoutButton.Clicked += EventClickedLogoutButton;
        }

        private Frame InitFrameLabels()
        {
            StackLayout stackLabels = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    nicknameLabel,
                    nomeLabel,
                    cognomeLabel,
                    luoghiRecensitiLabel,
                    iscrittoDalLabel
                },
            };
            Frame frameLabel = new Frame
            {
                BorderColor = Color.Green,
                Content = stackLabels
            };
            return frameLabel;
        }

        private Frame InitFrameButtons()
        {
            StackLayout stackButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    recensioniPersonaliButton,
                    logoutButton
                }
            };
            Frame frameButtons = new Frame
            {
                BorderColor = Color.Green,
                Content = stackButtons
            };
            return frameButtons;
        }

        private void InitMenuLogin()
        {
            menuLogin = new MenuLogin();
        }

        private void InitEffettuareAccessoLabel()
        {
            effettuareAccessoLabel = new Label
            {
                Text = "Effettuare l'accesso",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
        }

        private void InitLoginButton()
        {
            loginButton = new Button
            {
                Text = "Login",
                BackgroundColor = Color.Green,
                TextColor = Color.White
            };
            loginButton.Clicked += EventClickedLoginButton;
        }

        private Frame InitFrameEffettuareAccesso()
        {
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    effettuareAccessoLabel,
                    loginButton
                }
            };
            Frame frame = new Frame
            {
                BorderColor = Color.Green,
                Content = stackLayout
            };
            return frame;
        }

        private Label nicknameLabel;
        private Label nomeLabel;
        private Label cognomeLabel;
        private Label luoghiRecensitiLabel;
        private Label iscrittoDalLabel;
        private Label effettuareAccessoLabel;
        private Button loginButton;
        private Button recensioniPersonaliButton;
        private Button logoutButton;
        private StackLayout stackLayoutAccessoEffettuato;
        private StackLayout stackLayoutEffettuareAccesso;
        private MenuLogin menuLogin;
        private bool primoAccesso;
    }
}
