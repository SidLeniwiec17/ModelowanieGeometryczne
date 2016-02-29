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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelowanieGeometryczne1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddSphere_Click(object sender, RoutedEventArgs e)
        {
            Point gestosc = new Point();
            SphereOption sphera = new SphereOption();
            sphera.ShowDialog();
            if (sphera.DialogResult == false)
                return;
            else if (sphera.DialogResult == true)
            {
                gestosc = sphera.gestosc;
                int iloscPunktow = (int)gestosc.X * (int)gestosc.Y + 2;
                CreateSphereMatrix(iloscPunktow, gestosc);
            }
        }

        public void CreateSphereMatrix(int iloscPunktow, Point gestosc)
        {
            float[,] matrix = new float[4, iloscPunktow];
            float r = 1.0f;
            int counter = 0;
            for(int i = 0 ; i < gestosc.X ; i++)
            {
                float x = (float)(((i + 1) * (1 / gestosc.X)) * 2 * Math.PI);
                for (int j = 0 ; j < gestosc.Y ; j++)
                {
                    float y = (float)(((j + 1) * (1 / gestosc.Y)) * Math.PI - (Math.PI / 2));

                    matrix[0, counter] = ParametricXphereX(r, x, y, gestosc);
                    matrix[1, counter] = ParametricXphereY(r, x, y, gestosc);
                    matrix[2, counter] = ParametricXphereZ(r, x, y, gestosc);
                    matrix[3, counter] = r;
                    
                    counter++;     
                }
            }


        }
        public float ParametricXphereX(float r, float fi, float gamma, Point gestosc)
        {
            return (float) (r * Math.Cos(gamma) * Math.Cos(fi));
        }
        public float ParametricXphereY(float r, float fi, float gamma, Point gestosc)
        {
            return (float)(r * Math.Cos(gamma) * Math.Sin(fi));
        }
        public float ParametricXphereZ(float r, float fi, float gamma, Point gestosc)
        {
            return (float)(r * Math.Cos(gamma));
        }
    }
}
