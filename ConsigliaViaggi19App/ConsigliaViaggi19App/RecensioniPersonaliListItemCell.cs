using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class RecensioniPersonaliListItemCell : ViewCell
    {
        public RecensioniPersonaliListItemCell()
        {
            Image image = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 150
            };
            image.SetBinding(Image.SourceProperty, "ImmagineStruttura");
            Label nome = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold
            };
            nome.SetBinding(Label.TextProperty, "NomeStruttura");
            Label valutazione = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.DarkGreen,
            };
            valutazione.SetBinding(Label.TextProperty, "ValutazioneText");
            Label stato = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.DarkGreen
            };
            stato.SetBinding(Label.TextProperty, "StatoText");
            StackLayout layoutLabels = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    nome,
                    valutazione,
                    stato
                }
            };
            StackLayout layoutCell = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    image,
                    layoutLabels
                }
            };
            View = layoutCell;
        }
    }
}
