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

namespace ModelowanieGeometryczne1
{
    /// <summary>
    /// Interaction logic for SphereOption.xaml
    /// </summary>
    public partial class SphereOption : Window
    {
        public Point gestosc;
        public SphereOption()
        {
            InitializeComponent();
            gestosc = new Point();
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (X.Text.Length < 1 || Y.Text.Length <1)
            {
                MessageBox.Show("Puste miejsce !");
                return;
            }
            int x, y;
            bool bx = int.TryParse(X.Text, out x);
            bool by = int.TryParse(Y.Text, out y);
            if (bx == true && by == true)
            {
                gestosc.X = x;
                gestosc.Y = y;

            }
            else
            {
                MessageBox.Show("Wartosc nie jest liczba");
                return;
            }

            if (gestosc.X < 5 || gestosc.Y < 5)
                MessageBox.Show("Siatka nie moze miec mniejszej gestosci niz 5");
            else if (gestosc.X > 50 || gestosc.Y > 50)
                MessageBox.Show("Siatka nie moze miec wiekszej gestosci niz 50");
            else
            {
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
