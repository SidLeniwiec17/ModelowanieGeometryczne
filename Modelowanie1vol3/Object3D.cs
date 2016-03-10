using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie1vol3
{
    public class Object3D
    {
        private List<System.Windows.Point> Depe;
        private List<VectorPoint> Vert;
        private float[,] SMatrix;
        private float[,] RMatrix;
        private float[,] TMatrix;
        private int tag;
        double alpX;
        double alpY;
        double alpZ;
        int iloscWar;
        int iloscOkre;
        float dPro;
        float mPro;



        public Object3D(List<System.Windows.Point> dep, List<VectorPoint> points)
        {
            // TODO: Complete member initialization 
            float[,] matrix = { 
                              {1.0f,0.0f,0.0f,0.0f},
                              {0.0f,1.0f,0.0f,0.0f},
                              {0.0f,0.0f,1.0f,0.0f},
                              {0.0f,0.0f,0.0f,1.0f}
                              };

            float[,] sssmatrix = { 
                              {0.2f,0.0f,0.0f,0.0f},
                              {0.0f,0.2f,0.0f,0.0f},
                              {0.0f,0.0f,0.2f,0.0f},
                              {0.0f,0.0f,0.0f,1.0f}
                              };


            this.Depe = dep;
            this.Vert = points;
            this.SMatrix = sssmatrix;
            this.RMatrix = matrix;
            this.TMatrix = matrix;

            alpX = 0.0;
            alpY = 0.0;
            alpZ = 0.0;

            iloscWar = 40;
            iloscOkre = 30;
            dPro = 1.0f;
            mPro = 0.5f;
        }
        public List<System.Windows.Point> Dependences
        {
            get { return Depe; }
            set { Depe = value; }
        }
        public List<VectorPoint> Vertices
        {
            get { return Vert; }
            set { Vert = value; }
        }

        public double alphaX
        {
            get { return alpX; }
            set { alpX = value; }
        }
        public double alphaY
        {
            get { return alpY; }
            set { alpY = value; }
        }
        public double alphaZ
        {
            get { return alpZ; }
            set { alpZ = value; }
        }
        public float[,] ScaleMatrix
        {
            get { return SMatrix; }
            set { SMatrix = value; }
        }
        public float[,] RotationMatrix
        {
            get { return RMatrix; }
            set { RMatrix = value; }
        }
        public float[,] TranslateMatrix
        {
            get { return TMatrix; }
            set { TMatrix = value; }
        }
        public int Tag
        {
            get { return tag; }
            set { tag = value; }
        }
        public int iloscWarstw
        {
            get { return iloscWar; }
            set { iloscWar = value; }
        }
        public int iloscOkregow
        {
            get { return iloscOkre; }
            set { iloscOkre = value; }
        }
        public float duzyPromien
        {
            get { return dPro; }
            set { dPro = value; }
        }
        public float malyPromien
        {
            get { return mPro; }
            set { mPro = value; }
        }
    }
}
