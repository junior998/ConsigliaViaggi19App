﻿using System;
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
            elencoRecensioniPersonali = new ElencoRecensioniPersonali();
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
                    DettagliUtente dettagliUtente = Queries.GetDettagliUtente(UtilityUtente.Nickname);
                    CaricaDatiUtente(dettagliUtente);
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

        private void CaricaDatiUtente(DettagliUtente dettagliUtente)
        {
            nomeLabel.Text = $"Nome: {dettagliUtente.Nome}";
            cognomeLabel.Text = $"Cognome: {dettagliUtente.Cognome}";
            iscrittoDalLabel.Text = $"Iscritto dal: {dettagliUtente.DataIscrizione.ToString("dd/MM/yyyy")}";
            luoghiRecensitiLabel.Text = $"Luoghi recensiti: {dettagliUtente.LuoghiRecensiti}";
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
            recensioniPersonaliButton.Clicked += EventRecensioniPersonaliButtonClicked;
        }

        private async void EventRecensioniPersonaliButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(elencoRecensioniPersonali);
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
        private ElencoRecensioniPersonali elencoRecensioniPersonali;
        private MenuLogin menuLogin;
        private bool primoAccesso;
    }
}
