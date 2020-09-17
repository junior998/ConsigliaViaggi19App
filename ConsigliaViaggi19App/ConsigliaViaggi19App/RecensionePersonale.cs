using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConsigliaViaggi19App
{
    class RecensionePersonale
    {
        public string ValutazioneText
        {
            get
            {
                return $"Valutazione data: {Recensione.Valutazione}";
            }
        }
        public string StatoText
        {
            get
            {
                return $"Stato recensione: {Stato}";
            }
        }
        public int IdStruttura { get; set; }
        public ImageSource ImmagineStruttura { get; set; }
        public string NomeStruttura { get; set; }
        public Recensione Recensione { get; set; }
        public string Stato { get; set; }
    }
}
