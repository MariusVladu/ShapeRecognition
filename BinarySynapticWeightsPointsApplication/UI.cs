using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.FeatureExtraction;
using BinarySynapticWeights.FeatureExtraction.Contracts;

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
        private static readonly CenterOfGravity CenterOfGravity = new CenterOfGravity();
        private static readonly RayIntersection RayIntersection = new RayIntersection();

        public UI()
        {
            InitializeComponent();

            graphics = drawingPictureBox.CreateGraphics();
            regularPen = new Pen(Color.Black, 2);
            significantPointBrush = (Brush)Brushes.Green;
            centerPointBrush = (Brush)Brushes.Red;
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
            var origin = new Point(190, 190);

            var size = 250;

            var bottomLeft = new Point(0 + 50, size);
            var bottomRight = new Point(size, size);
            var topLeft = new Point(0+ 50, 0+50);
            var topRight= new Point(size, 0+50);

            var pen = new Pen(centerPointBrush);
            var rayPen = new Pen((Brush)Brushes.Blue);

            graphics.DrawLine(pen, topLeft, topRight);
            graphics.DrawLine(pen, topRight, bottomRight);
            graphics.DrawLine(pen, bottomLeft, bottomRight);
            graphics.DrawLine(pen, bottomLeft, topLeft);

            for (int i = 0; i < 16; i++)
            {
                var radians = (Math.PI / 180) * 90 * 30;
                var rayPoint = new Point((int)(180 + 400 * Math.Cos(radians)), (int)(180 + 400 * Math.Sin(radians)));

                DrawPoint(rayPoint);
                graphics.DrawLine(rayPen, origin, rayPoint);

                var intersectionPoint1 = RayIntersection.GetIntersectionPoint(origin, rayPoint, topLeft, topRight);
                var intersectionPoint2 = RayIntersection.GetIntersectionPoint(origin, rayPoint, topRight, bottomRight);
                var intersectionPoint3 = RayIntersection.GetIntersectionPoint(origin, rayPoint, bottomLeft, bottomRight);
                var intersectionPoint4 = RayIntersection.GetIntersectionPoint(origin, rayPoint, bottomLeft, topLeft);
                DrawPoint(intersectionPoint1);
                DrawPoint(intersectionPoint2);
                DrawPoint(intersectionPoint3);
                DrawPoint(intersectionPoint4);

                //var intersectionPoint = RayIntersection.GetRayToLineSegmentIntersection(origin, radians, topLeft, topRight);

                //if(intersectionPoint == null)
                //    intersectionPoint = RayIntersection.GetRayToLineSegmentIntersection(origin, radians, topRight, bottomRight);

                //if (intersectionPoint == null)
                //    intersectionPoint = RayIntersection.GetRayToLineSegmentIntersection(origin, radians, bottomLeft, bottomRight);

                //if (intersectionPoint == null)
                //    intersectionPoint = RayIntersection.GetRayToLineSegmentIntersection(origin, radians, bottomLeft, topLeft);

                //if (intersectionPoint != null)
                //    graphics.DrawLine(rayPen, origin, intersectionPoint.Value);

                //DrawPoint(intersectionPoint);

            }
        }

        private void DrawPoint(Point? point)
        {
            if (point == null) return;

            graphics.FillRectangle(centerPointBrush, (float)point?.X - 4, (float)point?.Y - 4, 9, 9);
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
