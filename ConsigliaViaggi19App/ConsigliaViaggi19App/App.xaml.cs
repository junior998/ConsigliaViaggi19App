using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Configuration;
using Xamarin.Essentials;

namespace ConsigliaViaggi19App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MenuPrincipale())
            {
                BarBackgroundColor = Color.Green
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
