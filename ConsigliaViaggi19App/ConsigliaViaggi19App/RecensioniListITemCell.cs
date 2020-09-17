using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class RecensioniListITemCell : ViewCell
    {
        public RecensioniListITemCell()
        {
            Label identificativoUtente = new Label()
            {
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold, 
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Fill                
            };
            identificativoUtente.SetBinding(Label.TextProperty, "IdentificativoUtente");
            Label valutazione = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Fill
            };
            valutazione.SetBinding(Label.TextProperty, "ValutazioneText");
            Label commento = new Label()
            {
                TextColor = Color.DarkGreen,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Fill
            };
            commento.SetBinding(Label.TextProperty, "CommentoText");
            View = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    identificativoUtente,
                    valutazione,
                    commento
                }
            };
        }
    }
}
