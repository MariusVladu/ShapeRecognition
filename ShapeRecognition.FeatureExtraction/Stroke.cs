using ShapeRecognition.FeatureExtraction.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace ShapeRecognition.FeatureExtraction
{
    public class Stroke : IStroke
    {
        List<Point> pointsOnStroke;
        List<Point> significantPoints;
        int pointsSinceLastSignificantPoint;
        int indexOfTheLastSignificantPoint;
        public Stroke(Point startingPoint)
        {
            pointsOnStroke = new List<Point>();
            significantPoints = new List<Point>();
            pointsOnStroke.Add(startingPoint);
            significantPoints.Add(startingPoint);
            pointsSinceLastSignificantPoint = 0;
            indexOfTheLastSignificantPoint = 0;
        }

        public int GetNumberOfPoints()
        {
            return pointsOnStroke.Count;
        }

        public List<Point> GetSignificantPoints()
        {
            return significantPoints;
        }

        public Point GetLastSignificantPoint()
        {
            return significantPoints[significantPoints.Count - 1];
        }

        public void AddSignificantPoint(Point point)
        {
            significantPoints.Add(point);

            pointsSinceLastSignificantPoint = 0;
            indexOfTheLastSignificantPoint = 0;
        }

        public bool AppendPoint(Point point)
        {
            Point p1 = pointsOnStroke.Last();
            double distance = Math.Sqrt((point.X - p1.X) * (point.X - p1.X) + (point.Y - p1.Y) * (point.Y - p1.Y));

            if (distance < 10)
            {
                return false;
            }
            pointsOnStroke.Add(point);
            if (pointsSinceLastSignificantPoint <= 1)
            {
                pointsSinceLastSignificantPoint++;
                return false;
            }
            double MrMinusOne = CalculateMs(indexOfTheLastSignificantPoint, pointsOnStroke.Count - 2);
            double CurrentMr = CalculateMs(indexOfTheLastSignificantPoint, pointsOnStroke.Count - 1);

            Debug.WriteLine($"{(int)CurrentMr} : {(int)MrMinusOne}");
            // if last but least point is significant
            if (CurrentMr < MrMinusOne)
            {
                pointsSinceLastSignificantPoint = 1;
                significantPoints.Add(pointsOnStroke[pointsOnStroke.Count - 2]);
                indexOfTheLastSignificantPoint = pointsOnStroke.Count - 2;
                return true;
            }
            return false;
        }

        private double CalculateMs(int indexOfStartPoint, int indexOfEndPoint)
        {
            Point startPoint = pointsOnStroke[indexOfStartPoint];
            Point endPoint = pointsOnStroke[indexOfEndPoint];
            double segmentLength = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
            double error = 0;
            for (int i = indexOfStartPoint + 1; i <= indexOfEndPoint; i++)
            {
                Point intermediatePoint = pointsOnStroke[i];
                double firstTermOfNominator = (endPoint.Y - startPoint.Y) * intermediatePoint.X;
                double secondTermOfNominator = (endPoint.X - startPoint.X) * intermediatePoint.Y;
                double thirdTermOfNominator = endPoint.X * startPoint.Y;
                double fourthTermOfNominator = startPoint.X * endPoint.Y;
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
