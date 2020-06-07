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
                currentStroke.AppendPoint(currentPoint);
            }
        }

        private void drawingPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (currentStroke.GetNumberOfPoints() > 1)
            {
                strokes.Add(currentStroke);
            }
        }

        private void SignificantPointsButton_Click(object sender, EventArgs e)
        {
            List<Point> significantPoints = new List<Point>();
            foreach (Stroke s in strokes)
            {
                List<Point> significantPointsOnCurrentStroke = s.GetSignificantPoints();
                DrawSignificantPoints(significantPointsOnCurrentStroke);
            }

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
