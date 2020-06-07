using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.FeatureExtraction;
using BinarySynapticWeights.FeatureExtraction.Contracts;
using BinarySynapticWeights.FeatureExtraction.Entities;

namespace BinarySynapticWeightsPointsApplication
{
    public partial class UI : Form
    {
        private Graphics graphics;
        private Pen regularPen;
        private Brush significantPointBrush;
        private Brush centerPointBrush;
        private List<IStroke> strokes = new List<IStroke>();
        private IStroke currentStroke;
        private Point lastDrawnPoint;

        private IBSWTrainingAlgorithm bswTrainingAlgorithm;
        private IBSWNeuralNetwork bswNeuralNetwork;

        private Point centerOfGravityPoint;
        private List<Point> significantPointAfterRayIntersection;
        private static readonly CenterOfGravity CenterOfGravity = new CenterOfGravity();
        private static readonly RayIntersection RayIntersection = new RayIntersection(32);
        private static readonly FeatureExtraction FeatureExtraction = new FeatureExtraction();

        public UI()
        {
            InitializeComponent();

            graphics = drawingPictureBox.CreateGraphics();
            regularPen = new Pen(Color.Black, 2);
            significantPointBrush = (Brush)Brushes.Green;
            centerPointBrush = (Brush)Brushes.Blue;
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentStroke = new Stroke(e.Location);
                lastDrawnPoint = e.Location;
            }
        }

        private void drawingPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point currentPoint = e.Location;
                graphics.DrawLine(regularPen, lastDrawnPoint, currentPoint);
                lastDrawnPoint = currentPoint;
                bool isNewSignificantPoint = currentStroke.AppendPoint(currentPoint);
                if (isNewSignificantPoint)
                {
                    Point lastSignificant = currentStroke.GetLastSignificantPoint();
                    graphics.FillRectangle(significantPointBrush, lastSignificant.X - 4, lastSignificant.Y - 4, 9, 9);
                }
            }
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentStroke.GetNumberOfPoints() > 1)
            {
                strokes.Add(currentStroke);
            }
        }

        private void ResetCanvasButton_Click(object sender, EventArgs e)
        {
            drawingPictureBox.Refresh();
            strokes = new List<IStroke>();
        }

        private void DrawSignificantPoints(List<Point> significantPointsOnCurrentStroke)
        {
            foreach (Point p in significantPointsOnCurrentStroke)
            {
                graphics.FillRectangle(significantPointBrush, p.X - 4, p.Y - 4, 9, 9);
            }
        }

        private void CenterOfGravityButton_Click(object sender, EventArgs e)
        {
            centerOfGravityPoint = CenterOfGravity.GetCenterOfGravity(strokes);

            graphics.FillRectangle(centerPointBrush, centerOfGravityPoint.X - 4, centerOfGravityPoint.Y - 4, 9, 9);
        }

        private void showRaysButton_Click(object sender, EventArgs e)
        {
            significantPointAfterRayIntersection = RayIntersection.GetSignificanPointsAfterRayIntersection(strokes, centerOfGravityPoint);

            foreach (var point in significantPointAfterRayIntersection)
            {
                DrawPoint(point);
            }

            //var scale = 1000;
            //var divisions = 16;
            //var segments = RayIntersection.GetAllSegmentsFromStrokes(strokes);

            //var intervalAngles = (360.0 / divisions);
            //for (int i = 0; i < divisions; i++)
            //{
            //    var radians = (Math.PI / 180) * intervalAngles * i;
            //    var rayPoint = new Point((int)(centerOfGravityPoint.X + scale * Math.Cos(radians)), (int)(centerOfGravityPoint.Y + scale * Math.Sin(radians)));

            //    foreach (var segment in segments)
            //    {
            //        var intersectionPoint = RayIntersection.GetIntersectionPoint(centerOfGravityPoint, rayPoint, segment.Item1, segment.Item2);

            //        if (intersectionPoint != null && RayIntersection.IsPointOnSegment(segment.Item1, segment.Item2, intersectionPoint.Value))
            //        {
            //            DrawPoint(intersectionPoint.Value);
            //        }
            //    }
            //}
        }

        private void DrawPoint(Point point)
        {
            graphics.FillRectangle(centerPointBrush, (float)point.X - 4, (float)point.Y - 4, 9, 9);
        }

        private void ExtractFeaturesButton_Click(object sender, EventArgs e)
        {
            var segments = FeatureExtraction.GetSegments(significantPointAfterRayIntersection);
            var shapeFeatures = new ShapeFeatures();

            for (int i = 1; i < segments.Count; i++)
            {
                var segment1 = segments[i - 1];
                var segment2 = segments[i];


                graphics.DrawLine(new Pen((Brush)Brushes.LightBlue, 4), segment1.Item1, segment1.Item2);
                graphics.DrawLine(new Pen((Brush)Brushes.LightGreen, 4), segment2.Item1, segment2.Item2);

                var angleInRadians = FeatureExtraction.GetAngleBetweenSegments(segment1.Item2, segment1.Item1, segment2.Item1, segment2.Item2);
                var angle = Math.Abs(angleInRadians * 180 / Math.PI);
                if (angle > 180)
                    angle = 360 - angle;

                if (angle < 60)
                    shapeFeatures.AcuteAngles++;
                else if (angle < 105)
                    shapeFeatures.RightAngles++;
                else if (angle < 165)
                    shapeFeatures.WideAngles++;
                else if (angle < 195)
                    shapeFeatures.StraightAngles++;
                else
                    shapeFeatures.ReflexAngles++;
            }

        }


        /*
        private void ClassifyAllPixels()
        {
            var graphics = drawing.CreateGraphics();

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    try
                    {

                        var inputVector = Preprocessing.GetInputVectorFromCoordinates(i, j);
                        var predictedClass = bswModel.PredictClass(inputVector);

                        if (predictedClass == "first group of pixels")
                            DrawPoint(graphics, predictedFirstClassPen, i, j);

                        else if (predictedClass == "second group of pixels")
                            DrawPoint(graphics, predictedSecondClassPen, i, j);
                    }
                    catch (Exception e) { }
                }
            }
        }
        */

        /*
    private void DrawPoint(Graphics graphics, Pen pen, int x, int y)
    {
        graphics.DrawRectangle(pen, x * scale, y * scale, 4, 4);
        graphics.DrawRectangle(pen, x * scale, y * scale, 3, 3);
        graphics.DrawRectangle(pen, x * scale, y * scale, 2, 2);
        graphics.DrawRectangle(pen, x * scale, y * scale, 1, 1);
    }
    */
    }
}
