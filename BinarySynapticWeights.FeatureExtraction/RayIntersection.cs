using BinarySynapticWeights.FeatureExtraction.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction
{
    public class RayIntersection
    {
        private int divisions;
        private int scale = 1000;

        public RayIntersection(int divisions)
        {
            this.divisions = divisions;
        }

        public List<Point> GetSignificanPointsAfterRayIntersection(List<IStroke> shapeStrokes, Point centerOfGravity)
        {
            var significantPoints = new List<Point>();
            var segments = GetAllSegmentsFromStrokes(shapeStrokes);

            var intervalAngles = (360.0 / divisions);
            for (int i = 0; i < divisions; i++)
            {
                var radians = (Math.PI / 180) * intervalAngles * i;
                var rayPoint = new Point((int)(centerOfGravity.X + scale * Math.Cos(radians)), (int)(centerOfGravity.Y + scale * Math.Sin(radians)));

                foreach (var segment in segments)
                {
                    var intersectionPoint = GetIntersectionPoint(centerOfGravity, rayPoint, segment.Item1, segment.Item2);

                    if (intersectionPoint != null && IsPointOnSegment(segment.Item1, segment.Item2, intersectionPoint.Value))
                    {
                        significantPoints.Add(intersectionPoint.Value);
                    }
                }
            }

            return significantPoints;

        }

        public List<Tuple<Point, Point>> GetAllSegmentsFromStrokes(List<IStroke> shapeStrokes)
        {
            var segments = new List<Tuple<Point, Point>>();

            foreach (var stroke in shapeStrokes)
            {
                segments.AddRange(GetStrokeSegments(stroke));
            }

            return segments;
        }

        public List<Tuple<Point, Point>> GetStrokeSegments(IStroke stroke)
        {
            var significantPoints = stroke.GetSignificantPoints();
            var strokeSegments = new List<Tuple<Point, Point>>();

            for (int i = 1; i < significantPoints.Count; i++)
            {
                var p1 = significantPoints[i - 1];
                var p2 = significantPoints[i];

                strokeSegments.Add(new Tuple<Point, Point>(p1, p2));
            }

            return strokeSegments;
        }

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

        public double Distance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
        }

        public bool IsPointOnSegment(Point a, Point b, Point pointToCheck)
        {
            return Math.Abs(Distance(a, pointToCheck) + Distance(pointToCheck, b) - Distance(a, b)) < 0.1;
        }
    }
}
