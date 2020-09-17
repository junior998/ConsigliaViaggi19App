using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PCLAppConfig;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.IO;
using System.Diagnostics;

namespace ConsigliaViaggi19App
{
    static class Queries
    {
        public static List<Struttura> GetLuoghiTrovati(ParametriRicercaStrutture parametri)
        {
            StringBuilder sottoQueryUno = new StringBuilder($"select S.idStruttura, S.nome, S.immagine, S.tipo, S.latitudine, " +
                $"S.longitudine, S.descrizione, C.nome as nomeCitta " +
                $"from Strutture S, Citta C " +
                $"where S.idCitta = C.idCitta ");
            if (!(parametri.NomeStruttura is null))
                sottoQueryUno.Append($"and lower(S.Nome) like '%{parametri.NomeStruttura.Replace("'", "''")}%' ");
            if (parametri.TipoStruttura != "Qualsiasi struttura")
                sottoQueryUno.Append($"and S.Tipo = '{parametri.TipoStruttura}' ");
            if (parametri.Citta != "Posizione corrente")
                sottoQueryUno.Append($"and C.Nome = '{parametri.Citta}' ");
            StringBuilder sottoQueryDue = new StringBuilder($"select S.idStruttura, ISNULL(avg(R.valutazione), 0) as valutazioneMedia " +
               "from Strutture S left outer join Recensioni R on S.idStruttura = R.idStruttura " +
               "group by S.idStruttura");
            string query = "select * " +
                $"from ({sottoQueryUno}) TMP1, ({sottoQueryDue}) TMP2 " +
                $"where TMP1.idStruttura = TMP2.idStruttura and " +
                $"TMP2.valutazioneMedia >= {parametri.ValutazioneMediaMinima} and TMP2.valutazioneMedia <= {parametri.ValutazioneMediaMassima};";
            DataTable table = EseguiComando(query);
            if (parametri.IsFiltroPosizioneAttivo)
                return FiltraStrutturePerDistanza(parametri.PosizioneCorrente, table, parametri.DistanzaMinima, parametri.DistanzaMassima);
            return GetLuoghiTrovatiDaDataTable(parametri.PosizioneCorrente, table);
        }
        public static List<string> GetCitta()
        {
            string query = "select distinct nome " +
                           "from Citta";
            DataTable table = EseguiComando(query);
            List<string> citta = new List<string>();
            foreach (DataRow row in table.Rows)
                citta.Add(row["nome"].ToString());
            return citta;
        }

        public static List<string> GetTipiStrutture()
        {
            string query = "select distinct tipo " +
                           "from Strutture";
            DataTable table = EseguiComando(query);
            List<string> citta = new List<string>();
            foreach (DataRow row in table.Rows)
                citta.Add(row["tipo"].ToString());
            return citta;
        }

        public static List<Struttura> GetStruttureConsigliate(Location posizioneCorrente)
        {
            string query = "select * " +
                           "from(select S.idStruttura, S.nome, S.immagine, S.tipo, S.latitudine, " +
                           "S.longitudine, S.descrizione, C.nome as nomeCitta " +
                           "from Strutture S, Citta C " +
                           "where S.idCitta = C.idCitta) TMP1, " +
                           "(select S.idStruttura, ISNULL(avg(R.valutazione), 0) as valutazioneMedia " +
                           "from Strutture S left outer join Recensioni R on S.idStruttura = R.idStruttura " +
                           "group by S.idStruttura) TMP2 " +
                           "where TMP1.idStruttura = TMP2.idStruttura " +
                           "order by TMP2.valutazioneMedia desc;";
            DataTable table = EseguiComando(query);
            return FiltraStrutturePerDistanza(posizioneCorrente, table);
        }

        public static DataTable GetDettagliUtente(string nickname)
        {
            string query = "select U.nickname, U.nome, U.cognome, U.dataIscrizione " +
                           "from Utenti U " +
                           $"where U.nickname = '{nickname.Replace("'", "''")}'";
            return EseguiComando(query);
        }

        public static DataTable GetLuoghiRecensiti(string nickname)
        {
            string query = "select count(*) as luoghiRecensiti " +
                           "from(select U.nickname, U.nome, U.cognome, U.dataIscrizione " +
                                 "from Utenti U, Recensioni R " +
                                $"where  U.nickname = R.nicknameUtente and U.nickname = '{nickname.Replace("'", "''")}' " +
                                "group by U.nickname, U.nome, U.cognome, U.dataIscrizione) as TMP ";
            return EseguiComando(query);
        }

        public static bool IsAccountEsistente(string nickname, string password)
        {
            string query = "select count(*) as conta " +
                           "from Utenti U " +
                           $"where U.nickname = '{nickname.Replace("'", "''")}' and U.password = '{password.Replace("'", "''")}'";
            DataTable dataTable = EseguiComando(query);
            foreach (DataRow riga in dataTable.Rows)
            {
                int temp = int.Parse(riga["conta"].ToString());
                if (temp == 0)
                    return false;
            }
            return true;
        }

        public static bool IsAccountEsistente(string nickname)
        {
            string query = "select count(*) as conta " +
               "from Utenti U " +
               $"where U.nickname = '{nickname.Replace("'", "''")}'";
            DataTable dataTable = EseguiComando(query);
            foreach (DataRow riga in dataTable.Rows)
            {
                int temp = int.Parse(riga["conta"].ToString());
                if (temp == 0)
                    return false;
            }
            return true;
        }

        public static void CreaAccount(string nome, string cognome, string nickname, string password)
        {
            string query = "insert into Utenti (nome, cognome, nickname, password, dataIscrizione) values " +
                           $"('{nome.Replace("'", "''")}', '{cognome.Replace("'", "''")}', '{nickname.Replace("'", "''")}', " +
                           $"'{password.Replace("'", "''")}', '{DateTime.Now}')";
            EseguiModifica(query);
        }

        private static void EseguiModifica(string query)
        {
            SqlConnection connection = new SqlConnection(stringConnection);
            SqlCommand command = new SqlCommand(query, connection)
            {
                CommandTimeout = 15
            };
            connection.Open();
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        private static DataTable EseguiComando(string query)
        {
            SqlConnection connection = new SqlConnection(stringConnection);
            SqlCommand command = new SqlCommand(query, connection)
            {
                CommandTimeout = 15
            };
            connection.Open();
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            connection.Dispose();
            return table;
        }

        private static List<Struttura> GetLuoghiTrovatiDaDataTable(Location posizioneCorrente, DataTable table)
        {
            List<Struttura> struttureConsigliate = new List<Struttura>();
            foreach (DataRow row in table.Rows)
                 struttureConsigliate.Add(CreaStruttura(row, posizioneCorrente));
            return struttureConsigliate;
        }

        private static List<Struttura> FiltraStrutturePerDistanza(Location posizioneCorrente, DataTable table, double distanzaMinima, double distanzaMassima)
        {
            List<Struttura> struttureConsigliate = new List<Struttura>();
            foreach (DataRow row in table.Rows)
            {
                Location posizioneStruttura = new Location((double)row["latitudine"], (double)row["longitudine"]);
                double distanza = GetDistanza(posizioneCorrente, posizioneStruttura);
                if (distanzaMinima <= distanza && distanzaMassima >= distanza)
                    struttureConsigliate.Add(CreaStruttura(row, posizioneCorrente));
            }
            return struttureConsigliate;
        }

        private static List<Struttura> FiltraStrutturePerDistanza(Location posizioneCorrente, DataTable table)
        {
            List<Struttura> struttureConsigliate = new List<Struttura>();
            foreach (DataRow row in table.Rows)
            {
                Location posizioneStruttura = new Location((double)row["latitudine"], (double)row["longitudine"]);
                double distanza = GetDistanza(posizioneCorrente, posizioneStruttura);
                if (distanza <= DISTANZA_CHILOMETRI_LUOGHI_CONSIGLIATI)
                    struttureConsigliate.Add(CreaStruttura(row, posizioneCorrente));
            }
            return struttureConsigliate;
        }

        private static Struttura CreaStruttura(DataRow row, Location posizioneCorrente)
        {
            double latitudineStruttura = (double)row["latitudine"];
            double longitudineStruttura = (double)row["longitudine"];
            Location posizioneStruttura = new Location(latitudineStruttura, longitudineStruttura);
            double distanza = Math.Round(GetDistanza(posizioneCorrente, posizioneStruttura), 2);
            Struttura struttura = new Struttura()
            {
                Id = (int)row["idStruttura"],
                Nome = row["nome"].ToString(),
                Tipo = row["tipo"].ToString(),
                Latitudine = latitudineStruttura,
                Longitudine = longitudineStruttura,
                Descrizione = row["Descrizione"].ToString(),
                NomeCitta = row["nomeCitta"].ToString(),
                ValutazioneMedia = double.Parse(row["valutazioneMedia"].ToString()),
                Distanza = distanza,
                Immagine = ConvertImage(row["immagine"])
            };
            return struttura;
        }

        private static double GetDistanza(Location posizionePartenza, Location posizioneArrivo)
        {
            return Location.CalculateDistance(posizionePartenza, posizioneArrivo, DistanceUnits.Kilometers);
        }

        private static ImageSource ConvertImage(object obj)
        {
            byte[] immagine = (byte[])obj;
            return ImageSource.FromStream(() => new MemoryStream(immagine));
        }

        public static readonly string stringConnection = ConfigurationManager.AppSettings["MyConnection"];
        private const int DISTANZA_CHILOMETRI_LUOGHI_CONSIGLIATI = 100;
    }
}
