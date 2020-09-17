using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PCLAppConfig;

namespace ConsigliaViaggi19App
{
    static class Queries
    {
        public static DataTable GetDettagliUtente(string nickname)
        {
            string query = "select U.nickname, U.nome, U.cognome, U.dataIscrizione " +
                           "from Utenti U " +
                           $"where U.nickname = '{nickname}'";
            return EseguiComando(query);
        }

        public static DataTable GetLuoghiRecensiti(string nickname)
        {
            string query = "select count(*) as luoghiRecensiti " +
                           "from(select U.nickname, U.nome, U.cognome, U.dataIscrizione " +
                                 "from Utenti U, Recensioni R " +
                                $"where  U.nickname = R.nicknameUtente and U.nickname = '{nickname}' " +
                                "group by U.nickname, U.nome, U.cognome, U.dataIscrizione) as TMP ";
            return EseguiComando(query);
        }

        public static bool IsAccountEsistente(string nickname, string password)
        {
            string query = "select count(*) as conta " +
                           "from Utenti U " +
                           $"where U.nickname = '{nickname}' and U.password = '{password}'";
            DataTable dataTable = EseguiComando(query);
            foreach (DataRow riga in dataTable.Rows)
            {
                int temp = int.Parse(riga["conta"].ToString());
                if (temp == 0)
                    return false;
            }
            return true;
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

        public static readonly string stringConnection = ConfigurationManager.AppSettings["MyConnection"];
    }
}
