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
            menuIscrizione = new MenuIscrizione();
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
            if (UtilityUtente.IsUtenteConnesso)
                Navigation.PopAsync();
            else
            {
                nicknameEntry.Text = "";
                passwordEntry.Text = "";
            }
        }

        private void EventClickedLoginButton(object sender, EventArgs eventArgs)
        {
            try
            {
                if (nicknameEntry.Text.Length == 0 || passwordEntry.Text.Length <= UtilityUtente.LunghezzaPassword)
                    DisplayAlert("Errore", "Inserire Nickname e password validi", "Ok");
                else if (!Queries.IsAccountEsistente(nicknameEntry.Text, passwordEntry.Text))
                    DisplayAlert("Errore", "Nickname o password non corretti", "Ok");
                else
                {
                    UtilityUtente.IsUtenteConnesso = true;
                    UtilityUtente.Nickname = nicknameEntry.Text;
                    Navigation.PopAsync();
                }
            }
            catch (SqlException)
            {
                DisplayAlert("Errore", "Connessione internet assente", "Ok");
            }
        }

        private void EventTappedIscrivitiLabel(object sender, EventArgs args)
        {
            Navigation.PushAsync(menuIscrizione);
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
            TapGestureRecognizer gesture = new TapGestureRecognizer();
            gesture.Tapped += EventTappedIscrivitiLabel;
            iscrivitiLabel.GestureRecognizers.Add(gesture);
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
        private MenuIscrizione menuIscrizione;
    }
}
