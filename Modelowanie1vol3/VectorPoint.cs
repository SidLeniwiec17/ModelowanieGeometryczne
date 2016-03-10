using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelowanie1vol3
{
    public class VectorPoint
    {
        private float Xval;
        private float Yval;
        private float Zval;
        private float Wval;

        public VectorPoint()
        {
            // TODO: Complete member initialization
            this.Xval = 0.0f;
            this.Yval = 0.0f;
            this.Zval = 0.0f;
            this.Wval = 0.0f;
        }
        public VectorPoint(float p1, float p2, float p3, float p4)
        {
            // TODO: Complete member initialization
            this.Xval = p1;
            this.Yval = p2;
            this.Zval = p3;
            this.Wval = p4;
        }
        public float X
        {
            get { return Xval; }
            set { Xval = value; }
        }
        public float Y
        {
            get { return Yval; }
            set { Yval = value; }
        }
        public float Z
        {
            get { return Zval; }
            set { Zval = value; }
        }
        public float W
        {
            get { return Wval; }
            set { Wval = value; }
        }
    }
}
