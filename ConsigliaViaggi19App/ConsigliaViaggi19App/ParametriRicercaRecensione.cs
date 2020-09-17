using System;
using System.Collections.Generic;
using System.Text;

namespace ConsigliaViaggi19App
{
    class ParametriRicercaRecensione
    {
        public int IdStruttura { get; set; }
        public DateTime DataDal { get; set; }
        public DateTime DataAl { get; set; }
        public int ValutazioneMinimo { get; set; }
        public int ValutazioneMassimo { get; set; }
    }
}
