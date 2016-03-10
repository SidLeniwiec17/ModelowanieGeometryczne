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

namespace Modelowanie1vol3
{
    /// <summary>
    /// Interaction logic for EditTorus.xaml
    /// </summary>
    public partial class EditTorus : Window
    {
        public int duze;
        public int male;
        public float R;
        public float r;
        public EditTorus(int d, int m, float P, float p)
        {
            InitializeComponent();
            duze = d;
            male = m;
            R = P;
            r = p;
            DuzeKola.Text = "" + duze;
            MaleKola.Text = "" + male;
            DuzyProm.Text = "" + P;
            MalyProm.Text = "" + p;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int tmpDuzeKola;
            bool raz = int.TryParse(DuzeKola.Text, out tmpDuzeKola);
            int tmpMaleKola;
            bool dwa = int.TryParse(MaleKola.Text, out tmpMaleKola);
            float tmpDuzyPromien;
            bool trzy = float.TryParse(DuzyProm.Text, out tmpDuzyPromien);
            float tmpMalyPromien;
            bool czter = float.TryParse(MalyProm.Text, out tmpMalyPromien);
            if (raz && dwa && trzy && czter)
            {
                if(tmpDuzeKola > 0 && tmpMaleKola > 0 && tmpDuzyPromien > 0 && tmpMalyPromien > 0 && tmpDuzyPromien > tmpMalyPromien)
                {
                    duze = tmpDuzeKola;
                    male = tmpMaleKola;
                    R = tmpDuzyPromien;
                    r = tmpMalyPromien;
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wartosci musza byc dodatnie, a duzy promien wiekszy od mniejszego.", "Blad !");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Ktores pole nie zawiera liczby.", "Blad !");
                return;
            }
        }
    }
}
