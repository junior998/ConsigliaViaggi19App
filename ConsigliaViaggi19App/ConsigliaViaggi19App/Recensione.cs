using System;
using System.Collections.Generic;
using System.Text;

namespace ConsigliaViaggi19App
{
    public class Recensione
    {
		public string IdentificativoUtente
        {
			get
            {
				if (VisibileConNickname)
					return $"Utente: {NicknameUtente}";
				return $"Utente: {NomeUtente}";
            }
        }

		public string ValutazioneText
        {
			get
            {
				return $"Valutazione: {Valutazione} stelle";
            }
        }

		//Il metodo restituisce la stringa "Commento: " + Commento
		//se Commento.Length <= 40
		//altrimenti vengono restituiti solo i primi 40 caratteri
		public string CommentoText
        {
			get
            {
				if (Commento.Length <= 40)
					return $"Commento: {Commento}";
				return $"Commento: " + Commento.Substring(0, 40) + "...";
			}
        }

		public int IdRecensione { get; set; }
		public int Valutazione { get; set; }
	    public string Commento { get; set; }
		public bool VisibileConNickname { get; set; }
		public DateTime DataCreazione { get; set; }
		public string NicknameUtente { get; set; }
		public string NomeUtente { get; set; }
		public string CognomeUtente { get; set; }
		public int IdStruttura { get; set; }
	}
}
