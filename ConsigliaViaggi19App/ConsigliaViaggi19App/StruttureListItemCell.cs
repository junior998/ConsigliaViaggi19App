using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class StruttureListItemCell : ViewCell
    {
        public StruttureListItemCell()
        {
            Image image = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                WidthRequest = 150
            };
            image.SetBinding(Image.SourceProperty, "Immagine");
            
            Label nome = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.DarkGreen,
                FontAttributes = FontAttributes.Bold
            };
            nome.SetBinding(Label.TextProperty, "Nome");

            Label distanza = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.DarkGreen
            };
            distanza.SetBinding(Label.TextProperty, "DistanzaTesto");

            Label valutazioneMedia = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                TextColor = Color.DarkGreen,
            };
            valutazioneMedia.SetBinding(Label.TextProperty, "ValutazioneMediaTesto");

            StackLayout layoutLabels = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    nome,
                    distanza,
                    valutazioneMedia
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
