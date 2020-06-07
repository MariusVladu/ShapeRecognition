using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySynapticWeightsPointsApplication
{
    public class Stroke
    {
        List<Point> pointsOnStroke;
        List<Point> significantPoints;
        bool areSignificantPointsComputed;
        public Stroke(Point startingPoint)
        {
            pointsOnStroke = new List<Point>();
            significantPoints = new List<Point>();
            pointsOnStroke.Add(startingPoint);
            significantPoints.Add(startingPoint);
            areSignificantPointsComputed = false;
        }

        public int GetNumberOfPoints()
        {
            return pointsOnStroke.Count;
        }

        public void AppendPoint(Point point)
        {
            pointsOnStroke.Add(point);
        }

        public List<Point> GetSignificantPoints()
        {
            if (!areSignificantPointsComputed)
            {
                ComputeSignificantPoints();
            }
            return significantPoints;
        }

        private void ComputeSignificantPoints()
        {
            if (pointsOnStroke.Count == 2)
            {
                significantPoints.Add(pointsOnStroke[1]);
                return;
            }

            double maximum = CalculateMs(1, 3); 

        }

        private double CalculateMs(int indexOfStartPoint, int indexOfEndPoint)
        {
            Point startPoint = pointsOnStroke[indexOfStartPoint];
            Point endPoint = pointsOnStroke[indexOfEndPoint];
            double segmentLength = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
            double error = 0;
            for (int i = indexOfStartPoint + 1; i < indexOfEndPoint; i++)
            {
                Point intermediatePoint = pointsOnStroke[i];
                double firstTermOfNominator = (endPoint.Y - startPoint.Y) * intermediatePoint.X;
                double secondTermOfNominator = (endPoint.X - startPoint.X) * intermediatePoint.Y;
                double thirdTermOfNominator = startPoint.X * endPoint.Y;
                double fourthTermOfNominator = endPoint.X * startPoint.Y;
                double nominator = Math.Abs(firstTermOfNominator - secondTermOfNominator + thirdTermOfNominator - fourthTermOfNominator);

                double firstTermOfDenominator = Math.Pow(endPoint.X - startPoint.X, 2);
                double secondTermOfDenominator = Math.Pow(endPoint.Y - startPoint.Y, 2);
                double denominator = Math.Sqrt(firstTermOfDenominator + secondTermOfDenominator);

                if (denominator == 0)
                {
                    continue;
                }

                error += nominator / denominator;

            }
            return segmentLength - error;
        }
    }
}
