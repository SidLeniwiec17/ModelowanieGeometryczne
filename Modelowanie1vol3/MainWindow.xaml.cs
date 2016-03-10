using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

namespace Modelowanie1vol3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class BoolStringClass
    {
        public string TheText { get; set; }
        public int TheValue { get; set; }
        public string Index { get; set; }
    }

    public partial class MainWindow : Window
    {
        public Bitmap mapa;
        public Bitmap CleanMapa;
        public Bitmap WorkspaceMapa;
        int Iwidth = 770;
        int Iheight = 580;
        int index = 0;
        public Object mojSender;
        public int selectedObject = -1;
        public List<Object3D> obiekty;
        public float currentR = 0.2f;
        public ObservableCollection<BoolStringClass> TheList { get; set; }
        public float[,] projectionMatrix = { 
                              {1.0f,0.0f,0.0f,0.0f},
                              {0.0f,1.0f,0.0f,0.0f},
                              {0.0f,0.0f,0.0f,1.0f},
                              {0.0f,0.0f,0.0f,1.0f}
                              };

        public MainWindow()
        {
            InitializeComponent();
            initBitmap();
            obiekty = new List<Object3D>();
            CreateCheckBoxList();
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        public void initBitmap()
        {
            int width = (int)CurrentImage.Width;
            int height = (int)CurrentImage.Height;

            if(width < 0 || height < 0)
            {
                width = Iwidth;
                height = Iheight;
            }
            else
            {
                Iwidth = width;
                Iheight = height;
            }

            mapa = new Bitmap(width, height);

            using (Graphics graph = Graphics.FromImage(mapa))
            {
                System.Drawing.Rectangle ImageSize = new System.Drawing.Rectangle(0, 0, width, height);
                graph.FillRectangle(System.Drawing.Brushes.White, ImageSize);
            }
            CleanMapa = new Bitmap(mapa);
            WorkspaceMapa = new Bitmap(mapa);
            CurrentImage.Source = ToBitmapSource(mapa);
        }
        private System.Windows.Media.ImageSource ToBitmapSource(Bitmap p_bitmap)
        {
            IntPtr hBitmap = p_bitmap.GetHbitmap();
            System.Windows.Media.ImageSource wpfBitmap;
            try
            {
                wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                //p_bitmap.Dispose();
                p_bitmap.Dispose();
                DeleteObject(hBitmap);
            }
            return wpfBitmap;
        }
        public void CreateCheckBoxList()
        {
            TheList = new ObservableCollection<BoolStringClass>();
            this.DataContext = this;
        }
        private void DodajSzescian_Click(object sender, RoutedEventArgs e)
        {
            InicjalizujSzescian();
            TheList.Add(new BoolStringClass { TheText = "Szescian # " + index, TheValue = 0, Index = "" + index });
            index++;
            Update();
        }
        private void DodajTorus_Click(object sender, RoutedEventArgs e)
        {
            InicjalizujTorus(40, 30, 1.0f, 0.5f);
            TheList.Add(new BoolStringClass { TheText = "Torus # " + index, TheValue = 0, Index = "" + index });
            index++;
            Update();
        }
        private void InicjalizujTorus(int iloscWarstw, int iloscOkregow , float R, float r)
        {
            float t, u;

            List<VectorPoint> torusPoints = new List<VectorPoint>();
            List<System.Windows.Point> torusVertex = new List<System.Windows.Point>();

            for(int w = 0 ; w < iloscWarstw ; w++)
            {
                u = (float)(w * ((2.0f * Math.PI) / iloscWarstw));

                for (int i = 0; i < iloscOkregow; i++)
                {
                    t = (float)(i * ((2.0f * Math.PI) / iloscOkregow));
                    VectorPoint tmpPoint = new VectorPoint();
                    tmpPoint.X = torusXparametr(t, R, r, u);
                    tmpPoint.Y = torusYparametr(t, R, r, u);
                    tmpPoint.Z = torusZparametr(t, R, r, u);
                    tmpPoint.W = 1.0f;

                    torusPoints.Add(tmpPoint);
                }
            }

            //poziomo
            for(int i = 0 ; i < iloscWarstw; i++)
            {
                for (int j = 0; j < iloscOkregow; j++)
                {
                    if(j == iloscOkregow - 1)
                        torusVertex.Add(new System.Windows.Point(i * iloscOkregow + j, i * iloscOkregow));
                    else
                        torusVertex.Add(new System.Windows.Point(i * iloscOkregow + j, i * iloscOkregow  + j + 1));
                }            
            }

            //pionowo
            for (int j = 0; j < iloscOkregow; j++)
            { 
                for (int i = 0; i < iloscWarstw; i++)
                {
                    if (i == iloscWarstw - 1)
                        torusVertex.Add(new System.Windows.Point(j + (i * iloscOkregow), j));
                    else
                        torusVertex.Add(new System.Windows.Point(j + (i * iloscOkregow), j + (i * iloscOkregow) + iloscOkregow));
                }
            }

            Object3D torus = new Object3D(torusVertex, torusPoints);
            torus.Tag = index;

            obiekty.Add(torus);
            rotateObject(0, 0, 0, obiekty.Count - 1);
            moveObject(0, 0, 50, obiekty.Count - 1);
        }
        private void EdytujTorus(int iloscWarstw, int iloscOkregow, float R, float r, int CurrentObiekt)
        {
            float t, u;

            obiekty[CurrentObiekt].Vertices.Clear();
            obiekty[CurrentObiekt].Dependences.Clear();
            obiekty[CurrentObiekt].duzyPromien = (float)R;
            obiekty[CurrentObiekt].malyPromien = (float)r;

            for (int w = 0; w < iloscWarstw; w++)
            {
                u = (float)(w * ((2.0f * Math.PI) / iloscWarstw));

                for (int i = 0; i < iloscOkregow; i++)
                {
                    t = (float)(i * ((2.0f * Math.PI) / iloscOkregow));
                    VectorPoint tmpPoint = new VectorPoint();
                    tmpPoint.X = torusXparametr(t, R, r, u);
                    tmpPoint.Y = torusYparametr(t, R, r, u);
                    tmpPoint.Z = torusZparametr(t, R, r, u);
                    tmpPoint.W = 1.0f;

                    obiekty[CurrentObiekt].Vertices.Add(tmpPoint);
                }
            }

            //poziomo
            for (int i = 0; i < iloscWarstw; i++)
            {
                for (int j = 0; j < iloscOkregow; j++)
                {
                    if (j == iloscOkregow - 1)
                        obiekty[CurrentObiekt].Dependences.Add(new System.Windows.Point(i * iloscOkregow + j, i * iloscOkregow));
                    else
                        obiekty[CurrentObiekt].Dependences.Add(new System.Windows.Point(i * iloscOkregow + j, i * iloscOkregow + j + 1));
                }
            }

            //pionowo
            for (int j = 0; j < iloscOkregow; j++)
            {
                for (int i = 0; i < iloscWarstw; i++)
                {
                    if (i == iloscWarstw - 1)
                        obiekty[CurrentObiekt].Dependences.Add(new System.Windows.Point(j + (i * iloscOkregow), j));
                    else
                        obiekty[CurrentObiekt].Dependences.Add(new System.Windows.Point(j + (i * iloscOkregow), j + (i * iloscOkregow) + iloscOkregow));
                }
            }

        }
        public float torusXparametr(float t, float R, float r, float u)
        {
            float result = 0.0f;
            result = (float)(Math.Cos(t) * (R + (r * Math.Cos(u))));
            return result;
        }
        public float torusYparametr(float t, float R, float r, float u)
        {
            float result = 0.0f;
            result = (float)(Math.Sin(t) * (R + (r * Math.Cos(u))));
            return result;
        }
        public float torusZparametr(float t, float R, float r, float u)
        {
            float result = 0.0f;
            result = (float)(r * Math.Sin(u));
            return result;
        }
        public void Update()
        {
            mapa = CleanMapa;
            drawLines();
        }
        private void InicjalizujSzescian()
        {
            List<VectorPoint> cubePoints = new List<VectorPoint>();
            List<System.Windows.Point> cubeVertex = new List<System.Windows.Point>();

            cubePoints.Add(new VectorPoint(-1.0f, -1.0f, -1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(1.0f, -1.0f, -1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(-1.0f, 1.0f, -1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(1.0f, 1.0f, -1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(-1.0f, -1.0f, 1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(1.0f, -1.0f, 1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(-1.0f, 1.0f, 1.0f, 1.0f));
            cubePoints.Add(new VectorPoint(1.0f, 1.0f, 1.0f, 1.0f));

            cubeVertex.Add(new System.Windows.Point(0, 1));
            cubeVertex.Add(new System.Windows.Point(1, 3));
            cubeVertex.Add(new System.Windows.Point(3, 2));
            cubeVertex.Add(new System.Windows.Point(2, 0));
            cubeVertex.Add(new System.Windows.Point(0, 4));
            cubeVertex.Add(new System.Windows.Point(1, 5));
            cubeVertex.Add(new System.Windows.Point(3, 7));
            cubeVertex.Add(new System.Windows.Point(2, 6));
            cubeVertex.Add(new System.Windows.Point(4, 5));
            cubeVertex.Add(new System.Windows.Point(5, 7));
            cubeVertex.Add(new System.Windows.Point(7, 6));
            cubeVertex.Add(new System.Windows.Point(6, 4));

            Object3D szescian = new Object3D(cubeVertex, cubePoints);
            szescian.Tag = index;

            szescian.iloscOkregow = -1;
            szescian.iloscWarstw = -1;
            obiekty.Add(szescian);
            rotateObject(0, 0, 0, obiekty.Count - 1);
            moveObject(0, 0, 50, obiekty.Count - 1);
           /* obiekty[obiekty.Count - 1].ScaleMatrix[0, 0] = obiekty[obiekty.Count - 1].ScaleMatrix[0, 0] + 100.5f;
            obiekty[obiekty.Count - 1].ScaleMatrix[1, 1] = obiekty[obiekty.Count - 1].ScaleMatrix[1, 1] + 100.5f;
            obiekty[obiekty.Count - 1].ScaleMatrix[2, 2] = obiekty[obiekty.Count - 1].ScaleMatrix[2, 2] + 100.5f;
            moveObject(100, 100, 0,  obiekty.Count - 1);*/

        }
        private void CheckBoxZone_Checked_1(object sender, RoutedEventArgs e)
        {
            RadioButton chkZone = (RadioButton)sender;
            for (int q = 0; q < obiekty.Count; q++)
            {
                if (obiekty.ElementAt(q).Tag == int.Parse(chkZone.Tag.ToString()))
                {
                    selectedObject = q;
                    break;
                }
            }
            //tutaj dane z list beda szly do GUI z kontrolkami
        }
        public void drawLines()
        {
            float fov = (float)(1.0f / Math.Tan(((90.0f/360.0f)* 2.0f *Math.PI) / 2.0f));
            int far = 1000, near = 100;
            float[,] ClipMatrix = { 
                              {(float)(fov * ((float)CurrentImage.ActualWidth / (float)CurrentImage.ActualHeight)),0.0f,0.0f,0.0f},
                              {0.0f,fov,0.0f,0.0f},
                              {0.0f,0.0f,(float)((far + near)/(far - near)),(float)((2 * far * near)/(near - far))},
                              {0.0f,0.0f,1.0f,0.0f}
                              };
            //tu czeba jakos mnozyc maciory
            using (Bitmap WorkspaceMapa = new Bitmap(CleanMapa))
            {
                System.Drawing.Pen blackPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);

                for (int i = 0; i < obiekty.Count; i++)
                {
                    
                    float[,] matrix = { 
                              {1.0f,0.0f,0.0f,0.0f},
                              {0.0f,1.0f,0.0f,0.0f},
                              {0.0f,0.0f,1.0f,0.0f},
                              {0.0f,0.0f,0.0f,1.0f}
                              };
                    //wmord/model
                    matrix = wymnozMacierze(i);
                    matrix = mnozenieMacierzy(matrix, ClipMatrix);
                    matrix = mnozenieMacierzy(matrix, projectionMatrix);
                    //mnozymy jeszcze przez projekcje i view o.0

                    for (int j = 0; j < obiekty[i].Dependences.Count; j++)
                    {
                        VectorPoint point1 = obiekty[i].Vertices[(int)obiekty[i].Dependences[j].X];
                        VectorPoint point2 = obiekty[i].Vertices[(int)obiekty[i].Dependences[j].Y];

                        point1 = mnozeniePunkt(point1, matrix);
                        point2 = mnozeniePunkt(point2, matrix);

                        float w1 = 1.0f;
                        if (point1.W != 0)
                            w1 = point1.W;

                        float w2 = 1.0f;
                        if (point2.W != 0)
                            w2 = point2.W;

                        int newX1 = (int)((point1.X * CurrentImage.ActualWidth) / (2.0f * w1) + (CurrentImage.ActualWidth / 2.0f));
                        int newY1 = (int)((point1.Y * CurrentImage.ActualHeight) / (2.0f * w1) + (CurrentImage.ActualHeight / 2.0f));
                        int newX2 = (int)((point2.X * CurrentImage.ActualWidth) / (2.0f * w2) + (CurrentImage.ActualWidth / 2.0f));
                        int newY2 = (int)((point2.Y * CurrentImage.ActualHeight) / (2.0f * w2) + (CurrentImage.ActualHeight / 2.0f));

                        using (var graphics = Graphics.FromImage(WorkspaceMapa))
                        {
                            graphics.DrawLine(blackPen, newX1, newY1, newX2, newY2);
                        }
                    }
                }
                mapa = WorkspaceMapa;
                CurrentImage.Source = ToBitmapSource(mapa);
            }
        }
        public VectorPoint mnozeniePunkt(VectorPoint point, float[,] Mmatrix)
        {
            float[] tmp = { (float)point.X, (float)point.Y, (float)point.Z, (float)point.W };
            VectorPoint result = new VectorPoint();
            result.X = Mmatrix[0, 0] * tmp[0] + Mmatrix[0, 1] * tmp[1] + Mmatrix[0, 2] * tmp[2] + Mmatrix[0, 3] * tmp[3];
            result.Y = Mmatrix[1, 0] * tmp[0] + Mmatrix[1, 1] * tmp[1] + Mmatrix[1, 2] * tmp[2] + Mmatrix[1, 3] * tmp[3];
            result.Z = Mmatrix[2, 0] * tmp[0] + Mmatrix[2, 1] * tmp[1] + Mmatrix[2, 2] * tmp[2] + Mmatrix[2, 3] * tmp[3];
            result.W = Mmatrix[3, 0] * tmp[0] + Mmatrix[3, 1] * tmp[1] + Mmatrix[3, 2] * tmp[2] + Mmatrix[3, 3] * tmp[3];

            return result;
        }
        private void RadioButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RadioButton chkZone = (RadioButton)sender;
            String name = chkZone.Content.ToString();
            mojSender = sender;
            e.Handled = true;
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
        private void MenuItem_Click_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Usuwamy obiekt ?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                RadioButton chkZone = (RadioButton)mojSender;
                int locindex = 0;

                for (int q = 0; q < obiekty.Count; q++)
                {
                    if (obiekty.ElementAt(q).Tag == int.Parse(chkZone.Tag.ToString()))
                    {
                        locindex = q;
                        break;
                    }
                }

                if (locindex >= 0)
                {
                    Console.WriteLine("usuwam...");
                    usunObiekt(locindex);
                }
            }

            if(obiekty.Count<1)
            {
                selectedObject = -1;
            }
        }
        private void MenuItem_Click_Edit(object sender, RoutedEventArgs e)
        {

            RadioButton chkZone = (RadioButton)mojSender;
            int locindex = 0;

            for (int q = 0; q < obiekty.Count; q++)
            {
                if (obiekty.ElementAt(q).Tag == int.Parse(chkZone.Tag.ToString()))
                {
                    locindex = q;
                    break;
                }
            }

            if (locindex >= 0)
            {
                Console.WriteLine("edycja...");
                EdytujObiekt(locindex);
            }
        }
        private void EdytujObiekt(int locIndex)
        {
            if(obiekty[locIndex].iloscOkregow<0)
                MessageBox.Show("Nie mozna edytowac szescianu.", "Uwaga !");
            else
            {
                EditTorus okno = new EditTorus(obiekty[locIndex].iloscWarstw, obiekty[locIndex].iloscOkregow, obiekty[locIndex].duzyPromien, obiekty[locIndex].malyPromien);
                okno.ShowDialog();
                if (okno.DialogResult == false)
                    return;
                else if (okno.DialogResult == true)
                {
                    obiekty[locIndex].iloscWarstw = okno.duze;
                    obiekty[locIndex].iloscOkregow = okno.male;
                    obiekty[locIndex].duzyPromien = okno.R;
                    obiekty[locIndex].malyPromien = okno.r;
                    EdytujTorus(obiekty[locIndex].iloscWarstw, obiekty[locIndex].iloscOkregow, obiekty[locIndex].duzyPromien, obiekty[locIndex].malyPromien, locIndex);
                }
            }

            Update();
        }
        private void usunObiekt(int locIndex)
        {
            TheList.RemoveAt(locIndex);
            obiekty.RemoveAt(locIndex);
            Update();
        }
        public float[,] wymnozMacierze(int objekt)
        {
            float[,] matrix = { 
                              {1.0f,0.0f,0.0f,0.0f},
                              {0.0f,1.0f,0.0f,0.0f},
                              {0.0f,0.0f,1.0f,0.0f},
                              {0.0f,0.0f,0.0f,1.0f}
                              };

            matrix = mnozenieMacierzy(obiekty[objekt].ScaleMatrix, obiekty[objekt].RotationMatrix);
            matrix = mnozenieMacierzy(matrix, obiekty[objekt].TranslateMatrix);

            return matrix;
        }
        public float[,] mnozenieMacierzy(float[,] Left, float[,] Right)
        {
            float[,] matrix = { 
                              {0.0f,0.0f,0.0f,0.0f},
                              {0.0f,0.0f,0.0f,0.0f},
                              {0.0f,0.0f,0.0f,0.0f},
                              {0.0f,0.0f,0.0f,0.0f}
                              };

            Parallel.For(0, 4, y =>
            {
                for (int x = 0; x < 4; x++)
                {
                    matrix[x, y] = Left[0, y] * Right[x, 0] + Left[1, y] * Right[x, 1] + Left[2, y] * Right[x, 2] + Left[3, y] * Right[x, 3];
                }
            });
            
            return matrix;
        }
        void ScalowanieLL(object sender, RoutedEventArgs e)
        {
            if (selectedObject >= 0)
            {
                //zoomObject(1.02f);
                obiekty[selectedObject].ScaleMatrix[0, 0] = obiekty[selectedObject].ScaleMatrix[0, 0] - 0.05f;
                obiekty[selectedObject].ScaleMatrix[1, 1] = obiekty[selectedObject].ScaleMatrix[1, 1] - 0.05f;
                obiekty[selectedObject].ScaleMatrix[2, 2] = obiekty[selectedObject].ScaleMatrix[2, 2] - 0.05f;
                Update();
            }
        }
        void ScalowanieL(object sender, RoutedEventArgs e)
        {
            if (selectedObject >= 0)
            {
                //zoomObject(1.02f);
                obiekty[selectedObject].ScaleMatrix[0, 0] = obiekty[selectedObject].ScaleMatrix[0, 0] - 0.005f;
                obiekty[selectedObject].ScaleMatrix[1, 1] = obiekty[selectedObject].ScaleMatrix[1, 1] - 0.005f;
                obiekty[selectedObject].ScaleMatrix[2, 2] = obiekty[selectedObject].ScaleMatrix[2, 2] - 0.005f;
                Update();
            }
        }
        void ScalowanieP(object sender, RoutedEventArgs e)
        {
            if (selectedObject >= 0)
            {
                //zoomObject(1.02f);
                obiekty[selectedObject].ScaleMatrix[0, 0] = obiekty[selectedObject].ScaleMatrix[0, 0] + 0.005f;
                obiekty[selectedObject].ScaleMatrix[1, 1] = obiekty[selectedObject].ScaleMatrix[1, 1] + 0.005f;
                obiekty[selectedObject].ScaleMatrix[2, 2] = obiekty[selectedObject].ScaleMatrix[2, 2] + 0.005f;
                Update();
            }
        }
        void ScalowaniePP(object sender, RoutedEventArgs e)
        {
            if (selectedObject >= 0)
            {
                //zoomObject(1.02f);
                obiekty[selectedObject].ScaleMatrix[0, 0] = obiekty[selectedObject].ScaleMatrix[0, 0] + 0.05f;
                obiekty[selectedObject].ScaleMatrix[1, 1] = obiekty[selectedObject].ScaleMatrix[1, 1] + 0.05f;
                obiekty[selectedObject].ScaleMatrix[2, 2] = obiekty[selectedObject].ScaleMatrix[2, 2] + 0.05f;
                Update();
            }
        }
        void ProjekcjaLL(object sender, RoutedEventArgs e)
        {
            //ZJBANE
                //zoomObject(1.02f);
               // float currentR = projectionMatrix[2, 3];
               // currentR = 1.0f / currentR;
               // currentR = currentR - 0.5f;
                currentR = currentR - 0.05f;
                projectionMatrix[2, 3] = 1.0f / (currentR );
                Update();
        }
        void ProjekcjaL(object sender, RoutedEventArgs e)
        {
            //zoomObject(1.02f);
           // float currentR = projectionMatrix[2, 3];
          //  currentR = 1.0f / currentR;
          //  currentR = currentR - 0.02f;
            currentR = currentR - 0.002f;
            projectionMatrix[2, 3] = 1.0f / (currentR );
            Update();
        }
        void ProjekcjaP(object sender, RoutedEventArgs e)
        {
            //zoomObject(1.02f);
           // float currentR = projectionMatrix[2, 3];
           // currentR = 1.0f / currentR;
          //  currentR = currentR + 0.02f;
            currentR = currentR + 0.002f;
            projectionMatrix[2, 3] = 1.0f / (currentR );
            Update();
        }
        void ProjekcjaPP(object sender, RoutedEventArgs e)
        {
            //zoomObject(1.02f);
            //float currentR = projectionMatrix[2, 3];
          //  currentR = 1.0f / currentR;
          //  currentR = currentR + 0.5f;
            currentR = currentR + 0.05f;
            projectionMatrix[2, 3] = 1.0f / (currentR);
            Update();
        }
        void rotateObject(int X, int Y, int Z, int obiekt)
        {
            if (obiekt >= 0)
            {
                double alpha = (2 * (double)X / 360.0) * 2.0 * Math.PI;
                double beta = (2 * (double)Y / 360.0) * 2.0 * Math.PI;
                double gamma = (2 * (double)Z / 360.0) * 2.0 * Math.PI;

                obiekty[obiekt].alphaX += alpha;
                obiekty[obiekt].alphaY += beta;
                obiekty[obiekt].alphaZ += gamma;

                float[,] Xmatrix = { {1.0f, 0.0f, 0.0f, 0.0f},
                              {0.0f, (float)Math.Cos(obiekty[obiekt].alphaX), (float)Math.Sin(obiekty[obiekt].alphaX), 0.0f},
                              {0.0f, -(float)Math.Sin(obiekty[obiekt].alphaX), (float)Math.Cos(obiekty[obiekt].alphaX), 0.0f},
                              {0.0f, 0.0f, 0.0f, 1.0f}};

                float[,] Ymatrix = { {(float)Math.Cos(obiekty[obiekt].alphaY), 0.0f, -(float)Math.Sin(obiekty[obiekt].alphaY), 0.0f},
                              {0.0f, 1.0f, 0.0f, 0.0f},
                              {(float)Math.Sin(obiekty[obiekt].alphaY),0.0f, (float)Math.Cos(obiekty[obiekt].alphaY), 0.0f},
                              {0.0f, 0.0f, 0.0f, 1.0f}};

                float[,] Zmatrix = { {(float)Math.Cos(obiekty[obiekt].alphaZ), (float)Math.Sin(obiekty[obiekt].alphaZ), 0.0f, 0.0f},
                              {-(float)Math.Sin(obiekty[obiekt].alphaZ), (float)Math.Cos(obiekty[obiekt].alphaZ), 0.0f, 0.0f},
                              {0.0f,0.0f, 1.0f, 0.0f},
                              {0.0f, 0.0f, 0.0f, 1.0f}};

                float[,] Rotmatrix = mnozenieMacierzy(Xmatrix, Ymatrix);
                Rotmatrix = mnozenieMacierzy(Rotmatrix, Zmatrix);
                obiekty[obiekt].RotationMatrix = Rotmatrix;
            }
        }
        void moveObject(int X, int Y, int Z, int obiekt)
        {
            if (obiekt >= 0)
            {
                float[,] Mmatrix = { {1.0f, 0.0f, 0.0f, 0.02f * X},
                              {0.0f, 1.0f, 0.0f, 0.02f * Y},
                              {0.0f, 0.0f, 1.0f, 0.02f * Z},
                              {0.0f, 0.0f, 0.0f, 1.0f}};

                obiekty[obiekt].TranslateMatrix = mnozenieMacierzy(Mmatrix, obiekty[obiekt].TranslateMatrix);
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
                rotateObject(0, -1, 0, selectedObject);

            if (e.Key == Key.Up)
                rotateObject(0, 1, 0, selectedObject);

            if (e.Key == Key.Left)
                rotateObject(-1, 0, 0, selectedObject);

            if (e.Key == Key.Right)
                rotateObject(1, 0, 0, selectedObject);

            if (e.Key == Key.O)
                rotateObject(0, 0, 1, selectedObject);

            if (e.Key == Key.P)
                rotateObject(0, 0, -1, selectedObject);

            
            if (e.Key == Key.D)
                moveObject(1, 0, 0, selectedObject);

            if (e.Key == Key.A)
                moveObject(-1, 0, 0, selectedObject);

            if (e.Key == Key.S)
                moveObject(0, 1, 0, selectedObject);

            if (e.Key == Key.W)
                moveObject(0, -1, 0, selectedObject);

            if (e.Key == Key.Q)
                moveObject(0, 0, 1, selectedObject);

            if (e.Key == Key.E)
                moveObject(0, 0, -1, selectedObject);

            Update();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            initBitmap();
            Update();
        }
    }
}
