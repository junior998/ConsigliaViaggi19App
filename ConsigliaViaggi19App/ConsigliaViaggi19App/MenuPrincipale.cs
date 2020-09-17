using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Data.SqlClient;
using Xamarin.Essentials;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsigliaViaggi19App
{
    class MenuPrincipale : TabbedPage
    {
        public MenuPrincipale()
        {
            Init();
        }

        private async void Init()
        {
            await CheckPermissionLocalization();
            InitComponents();
        }

        private async Task CheckPermissionLocalization()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if(status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if(status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Errore", "Permesso alla posizione negato", "Ok");
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void InitComponents()
        {
            menuLuoghiConsigliati = new MenuLuoghiConsigliati()
            {
                Title = "Luoghi Consigliati"
            };
            menuRicercaLuoghi = new MenuRicercaLuoghi()
            {
                Title = "Ricerca Luoghi"
            };
            menuAccount = new MenuAccount()
            {
                Title = "Menu Accout"
            };
            Children.Add(menuLuoghiConsigliati);
            Children.Add(menuRicercaLuoghi);
            Children.Add(menuAccount);
            BarBackgroundColor = Color.Green;
            SelectedTabColor = Color.Yellow;
            Title = "Consiglia Viaggi '19";
        }

        private MenuLuoghiConsigliati menuLuoghiConsigliati;
        private MenuRicercaLuoghi menuRicercaLuoghi;
        private MenuAccount menuAccount;
    }
}
