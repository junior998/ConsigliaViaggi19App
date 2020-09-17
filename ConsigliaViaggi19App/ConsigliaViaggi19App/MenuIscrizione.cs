using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class MenuIscrizione : ContentPage
    {
        public MenuIscrizione()
        {
            InitNomeEntry();
            InitCognomeEntry();
            InitNicknameEntry();
            InitPasswordEntry();
            InitLunghezzaPasswordLabel();
            InitIscrivitiButton();
            StackLayout layout = new StackLayout
            {
                Children =
                {
                    nomeEntry, cognomeEntry, nicknameEntry, passwordEntry,
                    lunghezzaPasswordLabel, iscrivitiButton
                }
            };
            Frame frame = new Frame
            {
                BorderColor = Color.Green,
                Content = layout
            };
            Content = frame;
            NavigationPage.SetHasBackButton(this, false);
            Title = "Menu Iscrizione";
        }

        protected override void OnAppearing()
        {
            nomeEntry.Text = "";
            cognomeEntry.Text = "";
            nicknameEntry.Text = "";
            passwordEntry.Text = "";
        }

        private bool EsisteEntryNonCorretta()
        {
            return (nomeEntry.Text.Length < lunghezzaMinimaCaselle || cognomeEntry.Text.Length < lunghezzaMinimaCaselle 
                    || nicknameEntry.Text.Length < lunghezzaMinimaCaselle);
        }

        private bool PasswordInseritaNonCorretta()
        {
            return (passwordEntry.Text.Length < lunghezzaMinimaPassword);
        }

        private void EventClickedIscrivitiButton(object sender, EventArgs args)
        {
            try
            {
                if (EsisteEntryNonCorretta())
                    DisplayAlert("Errore", $"Riempire tutte le caselle con almeno {lunghezzaMinimaCaselle} caratteri e " +
                        $"la password di almento {lunghezzaMinimaPassword} caratteri", "Ok");
                else if (PasswordInseritaNonCorretta())
                    DisplayAlert("Errore", $"Inserire password di almeno {lunghezzaMinimaPassword} caratteri", "Ok");
                else if (Queries.IsAccountEsistente(nicknameEntry.Text))
                    DisplayAlert("Errore", "Nickname già esistente", "Ok");
                else
                {
                    Queries.CreaAccount(nomeEntry.Text.ToUpper(), cognomeEntry.Text.ToUpper(), nicknameEntry.Text, passwordEntry.Text);
                    UtilityUtente.IsUtenteConnesso = true;
                    UtilityUtente.Nickname = nicknameEntry.Text;
                    DisplayAlert("Iscrizione", "Iscrizione all'app avvenuta con successo", "Ok");
                    Navigation.PopAsync();
                }
            }
            catch(SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
            }
        }

        private void InitNomeEntry()
        {
            nomeEntry = new Entry
            {
                Placeholder = "Nome",
                Keyboard = Keyboard.Text
            };
        }

        private void InitCognomeEntry()
        {
            cognomeEntry = new Entry
            {
                Placeholder = "Cognome",
                Keyboard = Keyboard.Text
            };
        }

        private void InitNicknameEntry()
        {
            nicknameEntry = new Entry
            {
                Placeholder = "Nickname",
                Keyboard = Keyboard.Text
            };
        }

        private void InitPasswordEntry()
        {
            passwordEntry = new Entry
            {
                Placeholder = "Password",
                Keyboard = Keyboard.Text,
                IsPassword = true
            };
        }

        private void InitLunghezzaPasswordLabel()
        {
            lunghezzaPasswordLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                Text = $"La password deve avere almeno {lunghezzaMinimaPassword} caratteri",
                TextColor = Color.Gray,
                HorizontalOptions = LayoutOptions.Start
            };
        }

        private void InitIscrivitiButton()
        {
            iscrivitiButton = new Button
            {
                BackgroundColor = Color.Green,
                Text = "Iscriviti",
                HorizontalOptions = LayoutOptions.End,
                TextColor = Color.White
            };
            iscrivitiButton.Clicked += EventClickedIscrivitiButton;
        }

        private const int lunghezzaMinimaPassword = 6;
        private const int lunghezzaMinimaCaselle = 3;
        private Entry nomeEntry;
        private Entry cognomeEntry;
        private Entry nicknameEntry;
        private Entry passwordEntry;
        private Label lunghezzaPasswordLabel;
        private Button iscrivitiButton;
    }
}
