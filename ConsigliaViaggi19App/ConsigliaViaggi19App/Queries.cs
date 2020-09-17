using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PCLAppConfig;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.IO;

namespace ConsigliaViaggi19App
{
    static class Queries
    {
        public static Struttura GetStruttura(int idStruttura)
        {
            string query = "select S.idStruttura, S.nome, S.immagine, S.tipo, S.latitudine, " +
                "S.longitudine, S.descrizione, C.nome as nomeCitta " +
                "from Strutture S, Citta C " +
                $"where S.idStruttura = {idStruttura} and " +
                "S.idCitta = C.idCitta;";
            DataTable table = EseguiComando(query);
            DataRow row = table.Rows[0];
            return CreaStruttura(row);
        }
        
        public static List<RecensionePersonale> GetRecensioniPersonali(string nicknameUtente)
        {
            string query = "select S.idStruttura, S.nome, S.immagine, R.valutazione, R.commento, R.stato, R.visibileConNickname, R.dataCreazione " +
                            "from Recensioni R, Strutture S " +
                            "where R.idStruttura = S.idStruttura and " +
                            $"R.nicknameUtente = '{nicknameUtente.Replace("'", "''")}' " +
                            $"order by R.dataCreazione desc;";
            DataTable table = EseguiComando(query);
            List<RecensionePersonale> recensioniPersonali = new List<RecensionePersonale>();
            foreach(DataRow row in table.Rows)
            {
                RecensionePersonale recensionePersonale = new RecensionePersonale
                {
                    IdStruttura = (int)row["idStruttura"],
                    NomeStruttura = row["nome"].ToString(),
                    ImmagineStruttura = ConvertImage(row["immagine"]),
                    Stato = row["stato"].ToString(),
                    Recensione = new Recensione()
                    {
                        Valutazione = (int)row["valutazione"],
                        Commento = row["commento"].ToString(),
                        VisibileConNickname = (bool)row["visibileConNickname"],
                        DataCreazione = (DateTime)row["dataCreazione"]
                    }
                };
                recensioniPersonali.Add(recensionePersonale);
            }
            return recensioniPersonali;
        }
        
        public static void CaricaRecensione(Recensione recensione)
        {
            int visibileConNickaname = (recensione.VisibileConNickname) ? 1 : 0;
            string query = "insert into Recensioni (valutazione, commento, stato, visibileConNickname, dataCreazione, nicknameUtente, idStruttura) values " +
                $"({recensione.Valutazione}, '{recensione.Commento}', 'in attesa', {visibileConNickaname}, '{recensione.DataCreazione.ToString("MM/dd/yyyy")}', " +
                $"'{recensione.NicknameUtente}', {recensione.IdStruttura})";
            EseguiModifica(query);
        }
        
        public static List<Recensione> GetRecensioni(ParametriRicercaRecensione parametri)
        {
            string query = $"select * " +
                $"from Utenti U, Recensioni R " +
                $"where U.Nickname = R.NicknameUtente and R.idStruttura = {parametri.IdStruttura} and " +
                $"R.valutazione >= {parametri.ValutazioneMinimo} and R.valutazione <= {parametri.ValutazioneMassimo} and " +
                $"R.dataCreazione <= '{parametri.DataAl.ToString("yyyy-MM-dd")}' and R.dataCreazione >= '{parametri.DataDal.ToString("yyyy-MM-dd")}' " +
                $"and R.stato = 'approvato';";
            DataTable table = EseguiComando(query);
            List<Recensione> recensioni = new List<Recensione>();
            foreach(DataRow row in table.Rows)
            {
                Recensione recensione = new Recensione()
                {
                    IdRecensione = (int)row["idRecensione"],
                    NicknameUtente = row["NickNameUtente"].ToString(),
                    NomeUtente = row["Nome"].ToString(),
                    CognomeUtente = row["Cognome"].ToString(),
                    VisibileConNickname = (bool)row["visibileConNickname"],
                    Commento = row["Commento"].ToString(),
                    DataCreazione = (DateTime)row["DataCreazione"],
                    Valutazione = (int)row["Valutazione"]
                };
                recensioni.Add(recensione);
            }
            return recensioni;
        }
        
        public static List<Tuple<int, int>> GetQuantitativoRecensioni(int idStruttura)
        {
            string query = $"select R.valutazione, count(R.idRecensione) as conta " +
                $"from Strutture S, Recensioni R " +
                $"where S.idStruttura = R.idStruttura and S.idStruttura = {idStruttura} and R.stato = 'approvato' " +
                $"group by R.valutazione " +
                $"order by R.valutazione desc;";
            DataTable table = EseguiComando(query);
            List<Tuple<int, int>> quantitaRecensioni = new List<Tuple<int, int>>();
            foreach(DataRow row in table.Rows)
            {
                int valutazione =(int)row["valutazione"];
                int conta = (int)row["conta"];
                quantitaRecensioni.Add(new Tuple<int, int>(valutazione, conta));
            }
            return quantitaRecensioni;
        }
        
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
            StringBuilder sottoQueryDue = new StringBuilder($"select S.idStruttura, CAST(ISNULL(avg(CAST(TMP.valutazione as DECIMAL(10, 2))), 0) as DECIMAL(10, 2)) as valutazioneMedia " +
               "from Strutture S left outer join " +
               "(select * " +
               "from Recensioni R " +
               "where R.stato = 'approvato') TMP on S.idStruttura = TMP.idStruttura " +
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
                           "(select S.idStruttura, CAST(ISNULL(avg(CAST(TMP.valutazione as DECIMAL(10, 2))), 0) as DECIMAL(10, 2)) as valutazioneMedia " +
                           "from Strutture S left outer join(select * " +
                           "from Recensioni R " +
                           "where R.stato = 'approvato') TMP on S.idStruttura = TMP.idStruttura " +
                           "group by S.idStruttura) TMP2 " +
                           "where TMP1.idStruttura = TMP2.idStruttura " +
                           "order by TMP2.valutazioneMedia desc;";
            DataTable table = EseguiComando(query);
            return FiltraStrutturePerDistanza(posizioneCorrente, table);
        }

        public static DettagliUtente GetDettagliUtente(string nickname)
        {
            string query = "select * " +
                            "from(select U.nickname, U.nome, U.cognome, U.dataIscrizione " +
                                  "from Utenti U " +
                                  $"where U.nickname = '{nickname.Replace("'", "''")}') TMP1, " +
                                  "(select U.nickname, count(R.idRecensione) as luoghiRecensiti " +
                                   "from Utenti U left outer join Recensioni R on U.nickname = R.nicknameUtente " +
                                   $"where U.nickname = '{nickname.Replace("'", "''")}' " +
                                   "group by U.nickname) TMP2 " +
                            "where TMP1.nickname = TMP2.nickname;";
            DataTable table = EseguiComando(query);
            DataRow row = table.Rows[0];
            DettagliUtente dettagliUtente = new DettagliUtente()
            {
                Nickname = row["nickname"].ToString(),
                Nome = row["nome"].ToString(),
                Cognome = row["cognome"].ToString(),
                DataIscrizione = (DateTime)row["dataIscrizione"],
                LuoghiRecensiti = (int)row["luoghiRecensiti"]
            };
            return dettagliUtente;
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
                           $"'{password.Replace("'", "''")}', '{DateTime.Now.ToString("MM/dd/yyyy")}')";
            EseguiModifica(query);
        }

        private static void EseguiModifica(string query)
        {
            SqlConnection connection = new SqlConnection(stringConnection);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        private static DataTable EseguiComando(string query)
        {
            SqlConnection connection = new SqlConnection(stringConnection);
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            connection.Dispose();
            return table;
        }

        private static List<Struttura> GetLuoghiTrovatiDaDataTable(Location posizioneCorrente, DataTable table)
        {
            List<Struttura> strutture = new List<Struttura>();
            foreach (DataRow row in table.Rows)
                 strutture.Add(CreaStruttura(row, posizioneCorrente));
            return strutture;
        }

        private static List<Struttura> FiltraStrutturePerDistanza(Location posizioneCorrente, DataTable table, double distanzaMinima, double distanzaMassima)
        {
            List<Struttura> strutture = new List<Struttura>();
            foreach (DataRow row in table.Rows)
            {
                Location posizioneStruttura = new Location((double)row["latitudine"], (double)row["longitudine"]);
                double distanza = GetDistanza(posizioneCorrente, posizioneStruttura);
                if (distanzaMinima <= distanza && distanzaMassima >= distanza)
                    strutture.Add(CreaStruttura(row, posizioneCorrente));
            }
            return strutture;
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

        private static Struttura CreaStruttura(DataRow row)
        {
            double latitudineStruttura = (double)row["latitudine"];
            double longitudineStruttura = (double)row["longitudine"];
            Struttura struttura = new Struttura()
            {
                Id = (int)row["idStruttura"],
                Nome = row["nome"].ToString(),
                Tipo = row["tipo"].ToString(),
                Latitudine = latitudineStruttura,
                Longitudine = longitudineStruttura,
                Descrizione = row["Descrizione"].ToString(),
                NomeCitta = row["nomeCitta"].ToString(),
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
