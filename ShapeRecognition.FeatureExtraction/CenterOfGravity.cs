using BinarySynapticWeights.FeatureExtraction.Contracts;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction
{
    public class CenterOfGravity
    {
        public Point GetCenterOfGravity(List<IStroke> shapeStrokes)
        {
            var segments = GetAllSegmentsFromStrokes(shapeStrokes);

            var allSegmentsLength = 0.0;
            var xSum = 0.0;
            var ySum = 0.0;

            foreach (var segment in segments)
            {
                var segmentLength = GetSegmentLength(segment.Item1, segment.Item2);
                allSegmentsLength += segmentLength;

                xSum += segmentLength * GetSegmentXAverage(segment.Item1, segment.Item2);
                ySum += segmentLength * GetSegmentYAverage(segment.Item1, segment.Item2);
            }

            var xCenter = (int)Math.Round(xSum / allSegmentsLength);
            var yCenter = (int)Math.Round(ySum / allSegmentsLength);

            return new Point(xCenter, yCenter);
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

        public double GetSegmentXAverage(Point p1, Point p2)
        {
            return (p1.X + p2.X) / 2.0;
        }

        public double GetSegmentYAverage(Point p1, Point p2)
        {
            return (p1.Y + p2.Y) / 2.0;
        }

        public double GetSegmentLength(Point p1, Point p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }
    }
}
