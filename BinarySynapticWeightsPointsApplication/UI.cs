using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BinarySynapticWeights;

namespace BinarySynapticWeightsPointsApplication
{
    public partial class UI : Form
    {
        private Graphics graphics;
        private Pen regularPen;
        private Brush significantPoinBrush;
        private List<Stroke> strokes = new List<Stroke>();
        private Stroke currentStroke;
        private Point lastDrawnPoint;
        // start of legacy members
        private int[,] matrix;
        private readonly int matrixSize = 64;
        private readonly int scale = 6;


        private Pen predictedFirstClassPen = new Pen(new SolidBrush(Color.LightGreen));
        private Pen predictedSecondClassPen = new Pen(new SolidBrush(Color.LightBlue));
        private Pen firstClassPen = new Pen(new SolidBrush(Color.DarkGreen));
        private Pen secondClassPen = new Pen(new SolidBrush(Color.DarkBlue));

        private NeuralNetworkTrainingAlgorithm binarySynapticWeights;
        private BSWModel bswModel;
        // end of legacy members

        public UI()
        {
            InitializeComponent();

            matrix = TrainingSets.GetThirdTrainingSet(matrixSize);
            graphics = drawingPictureBox.CreateGraphics();
            regularPen = new Pen(Color.Black, 2);
            significantPoinBrush = (Brush)Brushes.Green;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DrawTrainingPoints();
        }

        /*
        private void DrawTrainingPoints()
        {
            var graphics = drawing.CreateGraphics();

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (matrix[i, j] == 1)
                        DrawPoint(graphics, firstClassPen, i, j);

                    else if(matrix[i, j] == 2)
                        DrawPoint(graphics, secondClassPen, i, j);
                }
            }
        }
        */


        private void button2_Click(object sender, EventArgs e)
        {
            /*
            binarySynapticWeights = new NeuralNetworkTrainingAlgorithm();

            var trainingSamples = Preprocessing.GetTrainingSamplesFromMatrix(matrix);

            bswModel = binarySynapticWeights.Train(trainingSamples);

            ClassifyAllPixels();
            */
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentStroke = new Stroke(e.Location);
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
                graphics.FillRectangle(significantPoinBrush, p.X - 4, p.Y - 4, 9, 9);
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
