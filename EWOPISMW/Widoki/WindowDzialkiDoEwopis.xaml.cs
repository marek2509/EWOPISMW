using EWOPISMW.Infrstruktura;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EWOPISMW.Widoki
{
    /// <summary>
    /// Logika interakcji dla klasy WindowDzialkiDoEwopis.xaml
    /// </summary>
    public partial class WindowDzialkiDoEwopis : Window
    {
        public WindowDzialkiDoEwopis()
        {
            InitializeComponent();
            Plik.windowDzialkiDoEwopis = windowDzialkiDoEwopis;
            Plik.OdswierzScierzkeWPasku();

            if (Properties.Settings.Default.port3050 == true)
            {
                radioPort3050.IsChecked = true;
            }
            else
            {
                radioPort3051.IsChecked = true;
            }

        }

        private void Button_ClickWykonajSQL(object sender, RoutedEventArgs e)
        {
            try
            {
                dgMonitor.ItemsSource = BazaFB.Get_DataTable(textBoxMonitor.Text).AsDataView();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }



        }

        private void MenuItem_ClickUstawSciezke(object sender, RoutedEventArgs e)
        {
            Plik.PobierzSciezke();
        }
        List<TabelaKolumna> listaTabeleKolumny = new List<TabelaKolumna>();
        List<string> listaTabele = new List<string>();

        private void MenuItem_ClickPolaczZBaza(object sender, RoutedEventArgs e)
        {
            string pytanieSql = "select f.rdb$relation_name, f.rdb$field_name from rdb$relation_fields f join rdb$relations r on f.rdb$relation_name = r.rdb$relation_name and r.rdb$view_blr is null and(r.rdb$system_flag is null or r.rdb$system_flag = 0) order by 1, f.rdb$field_position; ";
            if (BazaFB.Get_DataTable(pytanieSql) == (null))
            {
                polaczMenu.Background = Brushes.Transparent;
            }
            else
            {
                DataTable dt = new DataTable();
                dt = BazaFB.Get_DataTable(pytanieSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listaTabeleKolumny.Add(new TabelaKolumna { Tabela = dt.Rows[i][0].ToString(), Kolumna = dt.Rows[i][1].ToString() });
                }

                listaTabele = listaTabeleKolumny.Select(x => x.Tabela).ToList().Distinct().ToList();
                listaTabel.ItemsSource = listaTabele;
                listaTabel.SelectedIndex = 0;
                listaKolumn.ItemsSource = listaTabeleKolumny.FindAll(x => x.Tabela == listaTabel.SelectedItem.ToString()).Select(x => x.Kolumna).ToList();
                polaczMenu.Background = Brushes.GreenYellow;
            }
        }

        private void RadioPort3050_Checked(object sender, RoutedEventArgs e)
        {
            radioPort3050.IsChecked = true;
            Properties.Settings.Default.port3050 = true;
            Properties.Settings.Default.Save();
        }

        private void RadioPort3051_Checked(object sender, RoutedEventArgs e)
        {
            radioPort3051.IsChecked = true;
            Properties.Settings.Default.port3050 = false;
            Properties.Settings.Default.Save();
        }

        public class TabelaKolumna
        {
            public string Tabela { get; set; }
            public string Kolumna { get; set; }
        }

        private void ListaTabel_Selected(object sender, SelectionChangedEventArgs e)
        {
            listaKolumn.ItemsSource = listaTabeleKolumny.FindAll(x => x.Tabela == listaTabel.SelectedItem.ToString()).Select(x => x.Kolumna).ToList();
        }

        private void Button_ClickDodajParametr(object sender, RoutedEventArgs e)
        {
            listBoxParametry.Items.Add(textboxParametr.Text.Trim());
        }

        private void Button_ClickUsunWybranyParametr(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(listBoxParametry.SelectedValue);
            if (listBoxParametry.Items.Count > 0 && listBoxParametry.SelectedIndex < listBoxParametry.Items.Count && listBoxParametry.SelectedIndex >= 0)
            {
                int selectedIdx = listBoxParametry.SelectedIndex;
                Console.WriteLine("bym usunal:" + listBoxParametry.SelectedValue);
                listBoxParametry.Items.RemoveAt(listBoxParametry.SelectedIndex);
                listBoxParametry.SelectedIndex = selectedIdx;
                if (listBoxParametry.SelectedIndex >= listBoxParametry.Items.Count)
                {
                    listBoxParametry.SelectedIndex = listBoxParametry.Items.Count - 1;
                }
            }
        }

        List<Modele.ModelDzialkiParametry> modelDzialkiParametry = new List<Modele.ModelDzialkiParametry>();
        private void Button_ClickOdczytajParametry(object sender, RoutedEventArgs e)
        {
            modelDzialkiParametry = Plik.odczytajParametry();

            dgParametry.ItemsSource = modelDzialkiParametry;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
         
            using (var connection = new FbConnection(BazaFB.PobierzConnectionString()))
            {
                connection.Open(); //UPDATE DZIALKI_N SET KW = CASE Id_Id  WHEN 660 THEN 'BI100000' else KW END where Id_Id IN(660)
                                   // FbCommand writeCommand = new FbCommand("INSERT INTO DZIALKA(rjdr, uid, id, idd, idobr, sidd, teryt, CTRL, status, osou, wrt, m2, dtu) VALUES((select id from jedn_rej jr where jr.ijr =@NrjednRej and ID_OBR =@nrObr), (select gen_id(UID_DZIALKA_G, 1)from rdb$database), (select gen_id(DZIALKI_G, 1)from rdb$database), @NrDz , @nrObr, '      .   ' || @NrDz || '/      ;      ', @TERYT, @NrDz, 0, 1, 0, 1, (select cast('NOW' as timestamp) from rdb$database ))", connection);
                FbCommand writeCommand = new FbCommand("INSERT INTO DZIALKA(rjdr,uid,id, idd, idobr, sidd, teryt, CTRL, STATUS, dtu, wrt, m2, DOWU, ZMA) VALUES((select id from jedn_rej jr where jr.ijr= @rjdr and ID_OBR= @idobr and sti =0), (select gen_id(UID_DZIALKA_G, 1)from rdb$database), (select gen_id(DZIALKI_G, 1)from rdb$database), @idd, @idobr, @sidd, @teryt, @CTRL, @STATUS, (select cast('NOW' as timestamp) from rdb$database), @wrt, @m2, @DOWUZMA, @DOWUZMA)", connection);
                writeCommand.CommandType = CommandType.Text;
                int ileDzialekWczytano = 0;
                try
                {
                    foreach (var item in modelDzialkiParametry)
                    {
                        writeCommand.Parameters.Clear();
                        int nrJednRej = item.NrJednRej;
                        int nrObrebu = item.IdObr;
                        string nrDz = item.Nrdz;

                        writeCommand.Parameters.Add("@rjdr", nrJednRej);
                        writeCommand.Parameters.Add("@idd", nrDz);
                        writeCommand.Parameters.Add("@idobr", nrObrebu);
                        writeCommand.Parameters.Add("@sidd", ModyfikacjaSIDD.GenerujSIDD(nrDz));
                        writeCommand.Parameters.Add("@teryt", "200509_2");
                        writeCommand.Parameters.Add("@CTRL", nrDz);
                        int StatusZero = checkBoxZmiana.IsChecked == true ? 2 : 0;
                        writeCommand.Parameters.Add("@STATUS", StatusZero);
                        int wartZero = 0;
                        writeCommand.Parameters.Add("@wrt", wartZero);
                        writeCommand.Parameters.Add("@m2", 1);

                        int? idZmiany = Modele.ModelZmiana.pobierzIdDo(listboxZmiany.SelectedValue == null ? "" : listboxZmiany.SelectedValue.ToString()); 
                        writeCommand.Parameters.Add("@DOWUZMA", idZmiany);

                       





                        writeCommand.ExecuteNonQuery();
                        ileDzialekWczytano++;
                    }
                }
                catch (Exception s)
                {

                    Console.WriteLine("excep S");
                  MessageBox.Show(s.Message);
                }


                connection.Close();
                MessageBox.Show("WŁALA. Załadowano działek: " + ileDzialekWczytano);
            }
        }
        List<Modele.ModelZmiana> listaModelZmian = new List<Modele.ModelZmiana>();
        private void ButtonPobierzZmiany_Click(object sender, RoutedEventArgs e)
        {
            listaModelZmian.Clear();
          DataTable dt =  BazaFB.Get_DataTable("select id, lp || '/' || rok || ' ' || sygn  from zmiany where zam  is null or zam = ''");
            BazaFB.Close_DB_Connection();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listaModelZmian.Add(new Modele.ModelZmiana() { idZmiany = Convert.ToInt32(dt.Rows[i][0]), NazwaZmiany = dt.Rows[i][1].ToString() });
            }
            listboxZmiany.ItemsSource = listaModelZmian.Select(x => x.PobierzDoListy());
           if(listaModelZmian.Select(x => x.PobierzDoListy()).ToList().Count == 0)
            {
                MessageBox.Show("Brak niezatwierdzonych zmian w programie EWOPIS.", "UWAGA!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                checkBoxZmiana.IsChecked = false;
            }
        }
    }
}
