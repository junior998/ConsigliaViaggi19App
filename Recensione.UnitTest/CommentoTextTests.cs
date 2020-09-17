using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsigliaViaggi19App.UnitTest
{
    [TestClass]
    public class CommentoTextTests
    {
        //White box tests
        [TestMethod]
        public void CommentoText_LunghezzaMinoreUguale40_RitornaStringaIntera()
        {
            Recensione recensione = new Recensione()
            {
                Commento = "Stringa corta"
            };
            string risultato = recensione.CommentoText;
            Assert.AreEqual(risultato, "Commento: Stringa corta");
        }

        [TestMethod]
        public void CommentoText_LunghezzaMaggiore40_RitornaSottoStringa()
        {
            Recensione recensione = new Recensione();
            StringBuilder stringa = new StringBuilder();
            for (int i = 0; i != 50; ++i)
                stringa.Append("s");
            recensione.Commento = stringa.ToString();
            string risultato = recensione.CommentoText;
            string risultatoAspettato = $"Commento: {stringa.ToString().Substring(0, 40)}...";
            Assert.AreEqual(risultato, risultatoAspettato);
        }

        //Black box tests
        [TestMethod]
        public void CommentoText_StringaVuota_RitornaStringaIntera()
        {
            Recensione recensione = new Recensione()
            {
                Commento = ""
            };
            string risultato = recensione.CommentoText;
            Assert.AreEqual(risultato, "Commento: ");
        }

        [TestMethod]
        public void CommentoText_StringaLunghezza40_RitornaStringaIntera()
        {
            StringBuilder stringa = new StringBuilder();
            for (int i = 0; i != 40; ++i)
                stringa.Append("s");
            Recensione recensione = new Recensione()
            {
                Commento = stringa.ToString()
            };
            string risultato = recensione.CommentoText;
            string risultatoAspettato = $"Commento: {stringa}";
            Assert.AreEqual(risultato, risultatoAspettato);
        }

        [TestMethod]
        public void CommentoText_StringaLunghezza39_RitornaStringaIntera()
        {
            StringBuilder stringa = new StringBuilder();
            for (int i = 0; i != 39; ++i)
                stringa.Append("s");
            Recensione recensione = new Recensione()
            {
                Commento = stringa.ToString()
            };
            string risultato = recensione.CommentoText;
            string risultatoAspettato = $"Commento: {stringa}";
            Assert.AreEqual(risultato, risultatoAspettato);
        }

        [TestMethod]
        public void CommentoText_StringLunghezza41_RitornaSottoStringa()
        {
            StringBuilder stringa = new StringBuilder();
            for (int i = 0; i != 41; ++i)
                stringa.Append("s");
            Recensione recensione = new Recensione()
            {
                Commento = stringa.ToString()
            };
            string risultato = recensione.CommentoText;
            string risultatoAspettato = $"Commento: {stringa.ToString().Substring(0, 40)}...";
            Assert.AreEqual(risultato, risultatoAspettato);
        }

        [TestMethod]
        public void CommentoText_ValoreNullo_LanciaEccezioneNullReferenceException()
        {
            Recensione recensione = new Recensione();
            Assert.ThrowsException<NullReferenceException>(() => recensione.CommentoText);
        }
    }
}
