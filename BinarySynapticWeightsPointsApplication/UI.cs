using System;
using System.Drawing;
using System.Windows.Forms;
using BinarySynapticWeights;

namespace BinarySynapticWeightsPointsApplication
{
    public partial class UI : Form
    {
        private int[,] matrix;
        private readonly int matrixSize = 64;
        private readonly int scale = 6;


        private Pen predictedFirstClassPen = new Pen(new SolidBrush(Color.LightGreen));
        private Pen predictedSecondClassPen = new Pen(new SolidBrush(Color.LightBlue));
        private Pen firstClassPen = new Pen(new SolidBrush(Color.DarkGreen));
        private Pen secondClassPen = new Pen(new SolidBrush(Color.DarkBlue));

        private NeuralNetworkTrainingAlgorithm binarySynapticWeights;
        private BSWModel bswModel;

        public UI()
        {
            InitializeComponent();

            matrix = TrainingSets.GetThirdTrainingSet(matrixSize);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawTrainingPoints();
        }

        private void DrawTrainingPoints()
        {
            var graphics = drawingPanel.CreateGraphics();

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

        private void button2_Click(object sender, EventArgs e)
        {
            binarySynapticWeights = new NeuralNetworkTrainingAlgorithm();

            var trainingSamples = Preprocessing.GetTrainingSamplesFromMatrix(matrix);

            bswModel = binarySynapticWeights.Train(trainingSamples);

            ClassifyAllPixels();
        }

        private void ClassifyAllPixels()
        {
            var graphics = drawingPanel.CreateGraphics();

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

        private void DrawPoint(Graphics graphics, Pen pen, int x, int y)
        {
            graphics.DrawRectangle(pen, x * scale, y * scale, 4, 4);
            graphics.DrawRectangle(pen, x * scale, y * scale, 3, 3);
            graphics.DrawRectangle(pen, x * scale, y * scale, 2, 2);
            graphics.DrawRectangle(pen, x * scale, y * scale, 1, 1);
        }
    }
}
