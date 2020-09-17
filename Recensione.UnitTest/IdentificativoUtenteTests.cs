using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsigliaViaggi19App.UnitTest
{
    [TestClass]
    public class IdentificativoUtenteTests
    {
        //white box tests
        [TestMethod]
        public void IdentificativoUtente_VisibileConNickname_RitornaNickname()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = true,
                NomeUtente = "nomeUtente",
                NicknameUtente = "nicknameUtente"
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: nicknameUtente");
        }

        [TestMethod]
        public void IdentificativoUtente_NonVisibileConNickname_RitornaNome()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = false,
                NomeUtente = "nomeUtente",
                NicknameUtente = "nicknameUtente"
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: nomeUtente");
        }

        //black box tests
        [TestMethod]
        public void IdentificativoUtente_VisibileConNicknameConNicknameVuoto_RitornaNicknameVuoto()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = true,
                NomeUtente = "nomeUtente",
                NicknameUtente = ""
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: ");
        }

        [TestMethod]
        public void IdentificativoUtente_NonVisibileConNicknameConNomeVuoto_RitornaNomeVuoto()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = false,
                NomeUtente = "",
                NicknameUtente = "nicknameUtente"
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: ");
        }

        [TestMethod]
        public void IdentificativoUtente_VisibileConNicknameConNicknameNullo_RitornaNicknameVuoto()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = true
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: ");
        }

        [TestMethod]
        public void IdentificativoUtente_NonVisibileConNicknameConUtenteNullo_RitornaUtenteVuoto()
        {
            Recensione recensione = new Recensione()
            {
                VisibileConNickname = false
            };
            string risultato = recensione.IdentificativoUtente;
            Assert.AreEqual(risultato, "Utente: ");
        }
    }
}
