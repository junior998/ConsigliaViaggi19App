using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Data.SqlClient;

namespace ConsigliaViaggi19App
{
    class MenuPrincipale : TabbedPage
    {
        public MenuPrincipale()
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
