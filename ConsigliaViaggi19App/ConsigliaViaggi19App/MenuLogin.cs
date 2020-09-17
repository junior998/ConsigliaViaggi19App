using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class MenuLogin : ContentPage
    {
        public MenuLogin()
        {
            InitLoginLabel();
            InitNicknameEntry();
            InitPasswordEntry();
            InitIscrivitiLabel();
            InitLoginButton();
            StackLayout formLayout = GetFormStackLayout();
            StackLayout stackLayoutForm = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    formLayout
                }
            };
            Frame frame = new Frame
            {
                BorderColor = Color.Green,
                Content = stackLayoutForm
            };
            Content = frame;
            Title = "Menu Login";
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override void OnAppearing()
        {
            nicknameEntry.Text = "";
            passwordEntry.Text = "";
        }

        private async void EventClickedLoginButton(object sender, EventArgs eventArgs)
        {
            try
            {
                if (nicknameEntry.Text.Length == 0 || passwordEntry.Text.Length <= UtilityUtente.LunghezzaPassword)
                    await DisplayAlert("Errore", "Nickname o password non corretti", "Ok");
                else if (!Queries.IsAccountEsistente(nicknameEntry.Text, passwordEntry.Text))
                    await DisplayAlert("Errore", "Account inesistente", "Ok");
                else
                {
                    UtilityUtente.IsUtenteConnesso = true;
                    UtilityUtente.Nickname = nicknameEntry.Text;
                    await Navigation.PopAsync();
                }
            }
            catch (SqlException)
            {
                await DisplayAlert("Errore", "Connessione internet assente", "Ok");
            }
        }

        private void InitLoginLabel()
        {
            loginLabel = new Label
            {
                Text = "Login",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
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

        private void InitIscrivitiLabel()
        {
            iscrivitiLabel = new Label
            {
                Text = "Iscriviti all'app",
                TextDecorations = TextDecorations.Underline
            };
        }

        private void InitLoginButton()
        {
            loginButton = new Button
            {
                Text = "Login",
                BackgroundColor = Color.Green
            };
            loginButton.Clicked += EventClickedLoginButton;
        }

        private StackLayout GetFormStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    loginLabel,
                    nicknameEntry,
                    passwordEntry,
                    iscrivitiLabel,
                    loginButton
                }
            };
            return stackLayout;
        }

        private Label loginLabel;
        private Entry nicknameEntry;
        private Entry passwordEntry;
        private Label iscrivitiLabel;
        private Button loginButton;
    }
}
