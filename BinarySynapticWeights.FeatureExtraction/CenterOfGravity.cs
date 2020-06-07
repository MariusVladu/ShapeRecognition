using System;
using System.Collections.Generic;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction
{
    public class CenterOfGravity
    {
        public Point GetCenterOfGravity(List<Point> significantShapePoints)
        {
            significantShapePoints.Add(significantShapePoints[0]);

            var totalShapeLength = GetTotalShapeLength(significantShapePoints);

            var xSum = 0.0;
            var ySum = 0.0;

            for (int i = 1; i < significantShapePoints.Count; i++)
            {
                var p1 = significantShapePoints[i - 1];
                var p2 = significantShapePoints[i];
                
                var segmentLength = GetSegmentLength(p1, p2);
                xSum += segmentLength * GetSegmentXAverage(p1, p2);
                ySum += segmentLength * GetSegmentYAverage(p1, p2);
            }

            significantShapePoints.RemoveAt(significantShapePoints.Count - 1);

            var xCenter = (int)Math.Round(xSum / totalShapeLength);
            var yCenter = (int)Math.Round(ySum / totalShapeLength);

            return new Point(xCenter, yCenter);
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

        public double GetTotalShapeLength(List<Point> shapePoints)
        {
            var totalLength = 0.0;

            for (int i = 1; i < shapePoints.Count; i++)
            {
                totalLength += GetSegmentLength(shapePoints[i - 1], shapePoints[i]);
            }

            return totalLength;
        }
    }
}
