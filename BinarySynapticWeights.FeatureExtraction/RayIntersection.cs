using System;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction
{
    public class RayIntersection
    {
        public Point? GetIntersectionPoint(Point a1, Point a2, Point b1, Point b2)
        {
            double A1 = a2.Y - a1.Y;
            double B1 = a1.X - a2.X;
            double C1 = A1 * a1.X + B1 * a1.Y;

            double A2 = b2.Y - b1.Y;
            double B2 = b1.X - b2.X;
            double C2 = A2 * b1.X + B2 * b1.Y;

            double numitor = A1 * B2 - A2 * B1;
            if (numitor == 0)
            {
                return null;
            }
            else
            {
                double x = (B2 * C1 - B1 * C2) / numitor;
                double y = (A1 * C2 - A2 * C1) / numitor;
                return new System.Drawing.Point(Convert.ToInt32(x), Convert.ToInt32(y));
            }
        }

    }
}
