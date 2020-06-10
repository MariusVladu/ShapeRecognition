using ShapeRecognition.FeatureExtraction.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ShapeRecognition.FeatureExtraction
{
    public class RayIntersection
    {
        private int divisions;
        private int scale = 1000;
        private double IntervalAngles => 360.0 / divisions;

        public RayIntersection(int divisions)
        {
            this.divisions = divisions;
        }

        public List<Point> GetSignificanPointsAfterRayIntersection(List<IStroke> shapeStrokes, Point centerOfGravity)
        {
            var intersections = new List<Point>();
            var segments = GetAllSegmentsFromStrokes(shapeStrokes);

            foreach (var segment in segments)
            {
                for (int i = 0; i < divisions / 2; i++)
                {
                    var radians = (Math.PI / 180) * IntervalAngles * i;
                    var raYPoint = new Point((int)(centerOfGravity.X + scale * Math.Cos(radians)), (int)(centerOfGravity.Y + scale * Math.Sin(radians)));

                    var intersection = GetIntersectionPoint(centerOfGravity, raYPoint, segment.Item1, segment.Item2);

                    if (intersection != null && IsPointOnSegment(segment.Item1, segment.Item2, intersection.Value))
                    {
                        intersections.Add(intersection.Value);
                    }
                }
            }
            var intersectionsWithoutExtensions = GetIntersectionsWithoutExtensions(intersections, centerOfGravity);

            var intersectionsMovedToClosestSignificanPoints = GetIntersectionsToClosestSignificantPoints(intersectionsWithoutExtensions, shapeStrokes);

            return GetOrderedPoints(intersectionsMovedToClosestSignificanPoints, centerOfGravity);
        }

        public List<Point> GetIntersectionsToClosestSignificantPoints(List<Point> raysIntersectionPoints, List<IStroke> strokes)
        {
            var significantPoints = GetAllSignificantPoints(strokes);

            var mergedPoints = raysIntersectionPoints.ToArray();

            foreach (var significantPoint in significantPoints)
            {
                int closestRayIntersectionPointIndex = 0;
                var minimumLength = double.MaxValue;

                for (int i = 0; i < raysIntersectionPoints.Count; i++)
                {
                    var length = GetSegmentLength(significantPoint, mergedPoints[i]);
                    if (length < minimumLength)
                    {
                        minimumLength = length;
                        closestRayIntersectionPointIndex = i; ;
                    }
                }

                mergedPoints[closestRayIntersectionPointIndex].X = significantPoint.X;
                mergedPoints[closestRayIntersectionPointIndex].Y = significantPoint.Y;
            }

            return new List<Point>(mergedPoints);
        }

        private List<Point> GetIntersectionsWithoutExtensions(List<Point> intersections, Point centerOfGravity)
        {
            var intersectionsWithoutExtensions = new List<Point>();

            for (int i = 0; i < divisions; i++)
            {
                var radians = (Math.PI / 180) * IntervalAngles * i;
                var raYPoint = new Point((int)(centerOfGravity.X + scale * Math.Cos(radians)), (int)(centerOfGravity.Y + scale * Math.Sin(radians)));

                var segmentsIntersectedByRay = new List<Point>();

                foreach (var intersectionPoint in intersections)
                {
                    if (IsPointOnSegment(centerOfGravity, raYPoint, intersectionPoint))
                    {
                        segmentsIntersectedByRay.Add(intersectionPoint);
                    }
                }

                if (segmentsIntersectedByRay.Any())
                {
                    var closestIntersectionPoint = segmentsIntersectedByRay.OrderBy(x => GetSegmentLength(x, centerOfGravity)).First();

                    intersectionsWithoutExtensions.Add(closestIntersectionPoint);
                }
            }

            return intersectionsWithoutExtensions;
        }

        public double GetSegmentLength(Point p1, Point p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        private List<Point> GetOrderedPoints(List<Point> points, Point centerOfGravity)
        {
            var orderedPoints = points.ToArray();

            for (int i = 0; i < orderedPoints.Length; i++)
            {
                for (int j = 0; j < orderedPoints.Length; j++)
                {
                    var a = orderedPoints[i];
                    var b = orderedPoints[j];

                    if (Less(a, b, centerOfGravity))
                    {
                        orderedPoints[i] = b;
                        orderedPoints[j] = a;
                    }
                }
            }

            return new List<Point>(orderedPoints);
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
            var allPoints = GetAllSignificantPoints(shapeStrokes);

            return GetSegments(allPoints);
        }

        public List<Point> GetAllSignificantPoints(List<IStroke> strokes)
        {
            var points = new List<Point>();

            foreach (var stroke in strokes)
            {
                var strokePoints = stroke.GetSignificantPoints();

                //if (points.Count > 1 && strokePoints.Count > 1)
                //{
                //    var intersection = GetIntersectionPoint(points[points.Count - 2], points[points.Count - 1], strokePoints[strokePoints.Count - 2], strokePoints[strokePoints.Count - 1]);

                //    if (intersection != null && IsPointOnSegment(points[points.Count - 2], points[points.Count - 1], intersection.Value))
                //    {
                //        points.Remove(points[points.Count - 1]);
                //        strokePoints.RemoveAt(0);

                //        points.Add(intersection.Value);
                //    }
                //}

                points.AddRange(strokePoints);
            }

            return points;
        }

        public List<Tuple<Point, Point>> GetSegments(List<Point> points)
        {
            var segments = new List<Tuple<Point, Point>>();

            for (int i = 1; i < points.Count; i++)
            {
                var p1 = points[i - 1];
                var p2 = points[i];

                segments.Add(new Tuple<Point, Point>(p1, p2));
            }

            return segments;
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
