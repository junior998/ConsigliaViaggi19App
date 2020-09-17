using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace ConsigliaViaggi19App
{
    class ParametriRicercaStrutture
    {
        public string NomeStruttura { get; set; }
        public string TipoStruttura { get; set; }
        public string Citta { get; set; }
        public double DistanzaMinima { get; set; }
        public double DistanzaMassima { get; set; }
        public int ValutazioneMediaMinima { get; set; }
        public int ValutazioneMediaMassima { get; set; }
        public bool IsFiltroPosizioneAttivo { get; set; }
        public Location PosizioneCorrente { get; set; }
    }
}
