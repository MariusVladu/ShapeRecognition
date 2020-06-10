using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BinarySynapticWeights;
using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.Entities;
using ShapeRecognition.FeatureExtraction;
using ShapeRecognition.FeatureExtraction.Contracts;
using ShapeRecognition.FeatureExtraction.Entities;

namespace ShapeRecognition
{
    public partial class UI : Form
    {
        private Graphics graphics;
        private Pen regularPen;
        private Brush significantPointBrush;
        private Brush centerPointBrush;
        private Pen rayPen;
        private Pen preprocessedSegmentsPen;
        private List<IStroke> strokes = new List<IStroke>();
        private IStroke currentStroke;
        private Point lastDrawnPoint;

        private Point centerOfGravityPoint;
        private List<Point> significantPointsAfterRayIntersection;
        private ShapeFeatures currentShapeFeatures;

        private static readonly int numberOfRays = 16;
        private static readonly CenterOfGravity CenterOfGravity = new CenterOfGravity();
        private static readonly RayIntersection RayIntersection = new RayIntersection(numberOfRays);
        private static readonly ShapeFeatureExtraction FeatureExtraction = new ShapeFeatureExtraction();
        private static readonly Preprocessing Preprocessing = new Preprocessing(numberOfRays);

        private IBSWTrainingAlgorithm bswTrainingAlgorithm;
        private IBSWNeuralNetwork bswNeuralNetwork;

        private static readonly Persistence Persistence = new Persistence();

        private List<Sample> initialTrainingSamples;
        private List<Sample> additionalTrainingSamples;

        public UI()
        {
            InitializeComponent();

            graphics = drawingPictureBox.CreateGraphics();
            regularPen = new Pen(Color.Black, 2);
            significantPointBrush = (Brush)Brushes.Green;
            centerPointBrush = (Brush)Brushes.Red;
            rayPen = new Pen(Color.Blue, 1);
            preprocessedSegmentsPen = new Pen(Color.LightBlue, 2);

            InitializedBSW();
        }

        private void InitializedBSW()
        {
            try
            {
                initialTrainingSamples = Persistence.ReadTrainingSamplesFromFile("SavedTrainingSamples\\initialTrainingSamples.json");

                bswTrainingAlgorithm = new BSWTrainingAlgorithm();
                var initialModel = bswTrainingAlgorithm.Train(initialTrainingSamples);

                bswNeuralNetwork = new BSWNeuralNetwork(initialModel);

                ShowBSWHiddenLayerNodes();
            }
            catch(Exception)
            {
                initialTrainingSamples = new List<Sample>();
            }

            additionalTrainingSamples = new List<Sample>();
        }

        private void drawingPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentStroke = new Stroke(e.Location);
                lastDrawnPoint = e.Location;

                graphics.FillRectangle(significantPointBrush, lastDrawnPoint.X - 4, lastDrawnPoint.Y - 4, 9, 9);

                strokes.Add(currentStroke);
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
                currentStroke.AddSignificantPoint(e.Location);

                graphics.FillRectangle(significantPointBrush, e.Location.X - 4, e.Location.Y - 4, 9, 9);
            }
        }

        private void ResetCanvasButton_Click(object sender, EventArgs e)
        {
            drawingPictureBox.Refresh();
            strokes = new List<IStroke>();

            acuteAnglesLabel.Text = "# Acute angles = ";
            rightAnglesLabel.Text = "# Right angles = ";
            wideAnglesLabel.Text = "# Wide angles = ";
            straightAnglesLabel.Text = "# Straight angles = ";
        }

        private void ComputeCenterOfGravity()
        {
            centerOfGravityPoint = CenterOfGravity.GetCenterOfGravity(strokes);

            graphics.FillRectangle(centerPointBrush, centerOfGravityPoint.X - 4, centerOfGravityPoint.Y - 4, 9, 9);
        }

        private void DrawSegmentsAfterRaysIntersection()
        {
            for (int i = 1; i < significantPointsAfterRayIntersection.Count; i++)
            {
                graphics.DrawLine(preprocessedSegmentsPen, significantPointsAfterRayIntersection[i - 1], significantPointsAfterRayIntersection[i]);
            }

            graphics.DrawLine(preprocessedSegmentsPen, significantPointsAfterRayIntersection.Last(), significantPointsAfterRayIntersection.First());
        }

        private void DrawPoint(Point point, int size)
        {
            graphics.FillRectangle(rayPen.Brush, (float)point.X - size / 2, (float)point.Y - size / 2, size, size);
        }

        private void ExtractFeaturesButton_Click(object sender, EventArgs e)
        {
            if (!strokes.Any()) return;

            ComputeCenterOfGravity();

            significantPointsAfterRayIntersection = RayIntersection.GetSignificanPointsAfterRayIntersection(strokes, centerOfGravityPoint);

            foreach (var point in significantPointsAfterRayIntersection)
            {
                graphics.DrawLine(rayPen, centerOfGravityPoint, point);
                DrawPoint(point, 9);
            }

            DrawSegmentsAfterRaysIntersection();

            currentShapeFeatures = FeatureExtraction.GetShapeFeatures(significantPointsAfterRayIntersection);

            acuteAnglesLabel.Text += currentShapeFeatures.AcuteAngles;
            rightAnglesLabel.Text += currentShapeFeatures.RightAngles;
            wideAnglesLabel.Text += currentShapeFeatures.WideAngles;
            straightAnglesLabel.Text += currentShapeFeatures.StraightAngles;

            recognizedShapeTypeLabel.Text = GetRecognizedClass();
        }

        private string GetRecognizedClass()
        {
            var inputVectorForBSW = Preprocessing.GetInputVectorFromShapeFeatures(currentShapeFeatures);

            try
            {
                return bswNeuralNetwork.PredictClass(inputVectorForBSW);
            }
            catch (Exception)
            {
                return "Unknown";
            }
        }

        private void trainRectangularShapeButton_Click(object sender, EventArgs e)
        {
            TrainCurrentShape(ShapeType.Rectangular);
        }

        private void trainTriangularShapeButton_Click(object sender, EventArgs e)
        {
            TrainCurrentShape(ShapeType.Triangular);
        }

        private void trainEllipticShapeButton_Click(object sender, EventArgs e)
        {
            TrainCurrentShape(ShapeType.Elliptic);
        }

        private void TrainCurrentShape(ShapeType shapeType)
        {
            if (!strokes.Any()) return;

            var trainingSample = Preprocessing.GetTrainingSampleFromShapeFeatures(currentShapeFeatures, shapeType);
            additionalTrainingSamples.Add(trainingSample);

            var allTrainingSamples = initialTrainingSamples.Concat(additionalTrainingSamples).ToList();

            try
            {
                bswTrainingAlgorithm = new BSWTrainingAlgorithm();
                var trainedModel = bswTrainingAlgorithm.Train(allTrainingSamples);
                bswNeuralNetwork = new BSWNeuralNetwork(trainedModel);
            }
            catch (Exception e) { }

            ShowBSWHiddenLayerNodes();
        }

        private void saveModelButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.FileName = Persistence.GetDefaultSaveFileName();
            fileDialog.Filter = "Json|*.json";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var allTrainingSamples = initialTrainingSamples.Concat(additionalTrainingSamples).ToList();

            Persistence.SaveTrainingSamplesToFile(allTrainingSamples, fileDialog.FileName);
        }

        private void loadModelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Json|*.json";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            initialTrainingSamples = Persistence.ReadTrainingSamplesFromFile(fileDialog.FileName);
            additionalTrainingSamples = new List<Sample>();

            bswTrainingAlgorithm = new BSWTrainingAlgorithm();
            var initialModel = bswTrainingAlgorithm.Train(initialTrainingSamples);

            bswNeuralNetwork = new BSWNeuralNetwork(initialModel);
        }

        private void ShowBSWHiddenLayerNodes()
        {
            hiddenLayerNodesLabel.Text = "" + bswNeuralNetwork.Model.HiddenNodes.Count;
        }
    }
}
