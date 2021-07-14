using EWOPISMW.Widoki;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EWOPISMW.Infrstruktura
{
    static class BazaFB
    {

        public static string PobierzConnectionString()
        {
            string port;
            if (Properties.Settings.Default.port3050)
            {
                port = "3050";
            }
            else
            {
                port = "3051";
            }

            string connectionString = @"User=" + Properties.Settings.Default.Login + ";Password=" + Properties.Settings.Default.Haslo + ";Database= " + Properties.Settings.Default.SciezkaFDB + "; DataSource=localhost; Port=" + port + ";Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true;" +
                                   "MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0;";
            return connectionString;
        }

        private static FbConnection Get_DB_Connection()
        {
            string cn_String = PobierzConnectionString();
            FbConnection cn_connection = new FbConnection(cn_String);

            if (cn_connection.State != ConnectionState.Open) cn_connection.Open();
            Console.WriteLine("state " + cn_connection.State);
            return cn_connection;          
        }

        public static DataTable Get_DataTable(string SQL_Text)
        {
            try
            {
                Plik.OdswierzScierzkeWPasku();
                FbConnection cn_connection = Get_DB_Connection();
                DataTable table = new DataTable();
                //FbTransaction transaction = cn_connection.BeginTransaction();

                FbDataAdapter adapter = new FbDataAdapter(SQL_Text, cn_connection);
                adapter.Fill(table);

                return table;
            }
            catch (Exception s)
            {
                MessageBox.Show(s.Message);
                Close_DB_Connection();
                try
                {
                    przejdzDoOknaLogowania(s.Message);
                }
                catch
                {
                    MessageBox.Show("Spróbuj zmienić port.", "Podpowiedź.",MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
                return null;
            }
        }

        public static void Execute_SQL(string SQL_command_execute)
        {
            FbConnection cn_connection = Get_DB_Connection();

            FbCommand cmd_Command = new FbCommand(SQL_command_execute, cn_connection);
            cmd_Command.ExecuteNonQuery();
        }

        public static void Close_DB_Connection()
        {
            string cn_String = PobierzConnectionString();

            FbConnection cn_connection = new FbConnection(cn_String);

            if (cn_connection.State != ConnectionState.Closed) cn_connection.Close();
        }

        static WindowLogowanieDoFDB windowLogowanie = new WindowLogowanieDoFDB();
        static void przejdzDoUstawLoginIHaslo()
        {
                windowLogowanie.Show();
        }

        static void przejdzDoOknaLogowania(string s)
        {
            if (s.Contains("password"))
            {
                przejdzDoUstawLoginIHaslo();
            }
        }
    }
}
