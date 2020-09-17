using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class Struttura
    {
        public string ValutazioneMediaTesto
        {
            get
            {
                if (ValutazioneMedia == 0)
                    return "Valutazione media: Nessuna Recensione";
                return $"Valutazione media: {ValutazioneMedia} stelle";
            }
        }

        public string DistanzaTesto
        {
            get
            {
                return $"Distanza: {Distanza}km";
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
        public double Distanza { get; set; }
        public double ValutazioneMedia { get; set; }
    }
}
