using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class Struttura
    {
        public double Distanza
        {
            get
            {
                return distanza;
            }
            set
            {
                distanza = value;
                DistanzaTesto = $"Distanza: {distanza}km";
            }
        }

        public double ValutazioneMedia
        {
            get
            {
                return valutazioneMedia;
            }
            set
            {
                valutazioneMedia = value;
                if (valutazioneMedia == 0)
                    ValutazioneMediaTesto = "Valutazione media: Nessuna Recensione";
                else
                    ValutazioneMediaTesto = $"Valutazione media: {valutazioneMedia} stelle";
            }
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public ImageSource Immagine { get; set; }
        public string Descrizione { get; set; }
        public double Latitudine { get; set; }
        public double Longitudine { get; set; }
        public string Tipo { get; set; }
        public string NomeCitta { get; set; }
        public string DistanzaTesto { get; set; }
        public string ValutazioneMediaTesto { get; set; }

        private double distanza;
        private double valutazioneMedia;
    }
}
