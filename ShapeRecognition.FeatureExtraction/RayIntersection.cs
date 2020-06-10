using ShapeRecognition.FeatureExtraction.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ShapeRecognition.FeatureExtraction
{
    public class RayIntersection
    {
        private int divisions;
        private int scale = 1000;

        public RayIntersection(int divisions)
        {
            this.divisions = divisions;
        }

        public List<Point> GetSignificanPointsAfterRayIntersection(List<IStroke> shapeStrokes, Point centerOfGravitY)
        {
            var significantPoints = new List<Point>();
            var segments = GetAllSegmentsFromStrokes(shapeStrokes);

            var intervalAngles = (360.0 / divisions);
            foreach (var segment in segments)
            {
                for (int i = 0; i < divisions / 2; i++)
                {
                    var radians = (Math.PI / 180) * intervalAngles * i;
                    var raYPoint = new Point((int)(centerOfGravitY.X + scale * Math.Cos(radians)), (int)(centerOfGravitY.Y + scale * Math.Sin(radians)));

                    var intersection = GetIntersectionPoint(centerOfGravitY, raYPoint, segment.Item1, segment.Item2);

                    if (intersection != null && IsPointOnSegment(segment.Item1, segment.Item2, intersection.Value))
                    {
                        significantPoints.Add(intersection.Value);
                    }
                }
            }

            for (int i = 0; i < significantPoints.Count; i++)
            {
                for (int j = 0; j < significantPoints.Count; j++)
                {
                    var a = significantPoints[i];
                    var b = significantPoints[j];

                    if(Less(a, b, centerOfGravitY))
                    {
                        significantPoints[i] = b;
                        significantPoints[j] = a;
                    }
                }
            }

            return significantPoints;
        }

        bool Less(Point a, Point b, Point center)
        {
            if (a.X - center.X >= 0 && b.X - center.X < 0)
                return true;
            if (a.X - center.X < 0 && b.X - center.X >= 0)
                return false;
            if (a.X - center.X == 0 && b.X - center.X == 0)
            {
                if (a.Y - center.Y >= 0 || b.Y - center.Y >= 0)
                    return a.Y > b.Y;
                return b.Y > a.Y;
            }

            // compute the cross product of vectors (center -> a) X (center -> b)
            int det = (a.X - center.X) * (b.Y - center.Y) - (b.X - center.X) * (a.Y - center.Y);
            if (det < 0)
                return true;
            if (det > 0)
                return false;

            // points a and b are on the same line from the center
            // check which point is closer to the center
            int d1 = (a.X - center.X) * (a.X - center.X) + (a.Y - center.Y) * (a.Y - center.Y);
            int d2 = (b.X - center.X) * (b.X - center.X) + (b.Y - center.Y) * (b.Y - center.Y);
            return d1 > d2;
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
                double X = (B2 * C1 - B1 * C2) / numitor;
                double Y = (A1 * C2 - A2 * C1) / numitor;
                return new Point(Convert.ToInt32(X), Convert.ToInt32(Y));
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
