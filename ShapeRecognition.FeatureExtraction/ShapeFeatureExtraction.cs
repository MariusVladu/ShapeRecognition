using ShapeRecognition.FeatureExtraction.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ShapeRecognition.FeatureExtraction
{
    public class ShapeFeatureExtraction
    {
        public ShapeFeatures GetShapeFeatures(List<Point> significantPoints)
        {
            var segments = GetSegments(significantPoints);
            var shapeFeatures = new ShapeFeatures();

            for (int i = 1; i < segments.Count; i++)
            {
                var segment1 = segments[i - 1];
                var segment2 = segments[i];

                var angleInRadians = GetAngleBetweenSegments(segment1.Item2, segment1.Item1, segment2.Item1, segment2.Item2);
                var angle = Math.Abs(angleInRadians * 180 / Math.PI);
                if (angle > 180)
                    angle = 360 - angle;

                if (angle < 75)
                    shapeFeatures.AcuteAngles++;
                else if (angle < 105)
                    shapeFeatures.RightAngles++;
                else if (angle < 165)
                    shapeFeatures.WideAngles++;
                else
                    shapeFeatures.StraightAngles++;
            }

            return shapeFeatures;
        }

        public List<Tuple<Point, Point>> GetSegments(List<Point> significantPoints)
        {
            significantPoints.Add(significantPoints.First());
            var segments = new List<Tuple<Point, Point>>();

            for (int i = 1; i < significantPoints.Count; i++)
            {
                var p1 = significantPoints[i - 1];
                var p2 = significantPoints[i];

                segments.Add(new Tuple<Point, Point>(p1, p2));
            }

            segments.Add(segments.First());

            return segments;
        }

        public double GetAngleBetweenSegments(Point p1, Point p2, Point p3, Point p4)
        {
            return Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) - Math.Atan2(p4.Y - p3.Y, p4.X - p3.X);
        }
    }
}
