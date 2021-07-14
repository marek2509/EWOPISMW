using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy WindowLogowanieDoFDB.xaml
    /// </summary>
    public partial class WindowLogowanieDoFDB : Window
    {
        public WindowLogowanieDoFDB()
        {
            InitializeComponent();
            textBoxLogin.Text = Properties.Settings.Default.Login;
            textBoxHaslo.Password = Properties.Settings.Default.Haslo;
        }
        private void ButtonZapiszLogIHaslo(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Login = textBoxLogin.Text;
            Properties.Settings.Default.Haslo = textBoxHaslo.Password;
            Properties.Settings.Default.Save();
            windowLogowanie.Close();
        }

        private void Button_Anuluj(object sender, RoutedEventArgs e)
        {
            windowLogowanie.Close();
        }
    }
}
