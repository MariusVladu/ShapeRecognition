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

        BinarySynapticWeightsAlgorithm binarySynapticWeights;

        public UI()
        {
            InitializeComponent();

            matrix = GetThirdTrainingSet();
        }

        private int[,] GetFirstTrainingSet()
        {
            matrix = new int[matrixSize, matrixSize];

            matrix[5, 5] = 1;
            matrix[10, 10] = 1;
            matrix[7, 7] = 1;
            matrix[7, 5] = 1;
            matrix[10, 15] = 1;
            matrix[20, 10] = 1;
            matrix[13, 7] = 1;
            matrix[20, 3] = 1;
            matrix[15, 15] = 1;
            matrix[14, 13] = 1;
            matrix[18, 17] = 1;

            matrix[50, 50] = 2;
            matrix[60, 60] = 2;
            matrix[57, 57] = 2;
            matrix[47, 51] = 2;
            matrix[50, 55] = 2;
            matrix[60, 50] = 2;
            matrix[53, 57] = 2;
            matrix[60, 63] = 2;
            matrix[48, 49] = 2;
            matrix[51, 51] = 2;
            matrix[47, 57] = 2;

            return matrix;
        }

        private int[,] GetSecondTrainingSet()
        {
            matrix = new int[matrixSize, matrixSize];

            matrix[5, 5] = 1;
            matrix[10, 10] = 1;
            matrix[7, 7] = 1;
            matrix[7, 5] = 1;
            matrix[10, 15] = 1;
            matrix[20, 10] = 1;
            matrix[13, 7] = 1;
            matrix[20, 3] = 1;
            matrix[15, 15] = 1;
            matrix[14, 13] = 1;
            matrix[18, 17] = 1;
            matrix[25, 25] = 1;
            matrix[30, 45] = 1;
            matrix[30, 35] = 1;
            matrix[35, 34] = 1;
            matrix[34, 38] = 1;

            matrix[30, 34] = 2;
            matrix[31, 32] = 2;
            matrix[20, 20] = 2;
            matrix[35, 39] = 2;
            matrix[37, 37] = 2;
            matrix[50, 50] = 2;
            matrix[60, 60] = 2;
            matrix[57, 57] = 2;
            matrix[47, 51] = 2;
            matrix[50, 55] = 2;
            matrix[60, 50] = 2;
            matrix[53, 57] = 2;
            matrix[60, 63] = 2;
            matrix[48, 49] = 2;
            matrix[51, 51] = 2;
            matrix[47, 57] = 2;

            return matrix;
        }

        private int[,] GetThirdTrainingSet()
        {
            matrix = new int[matrixSize, matrixSize];

            matrix[5, 5] = 1;
            matrix[10, 10] = 1;
            matrix[7, 7] = 1;
            matrix[7, 5] = 1;
            matrix[10, 15] = 1;
            matrix[20, 10] = 1;
            matrix[13, 7] = 1;
            matrix[20, 3] = 1;
            matrix[15, 15] = 1;
            matrix[14, 13] = 1;
            matrix[18, 17] = 1;
            matrix[25, 25] = 1;
            matrix[30, 45] = 1;
            matrix[30, 35] = 1;
            matrix[35, 34] = 1;
            matrix[34, 38] = 1;

            matrix[40, 7] = 2;
            matrix[41, 14] = 2;
            matrix[42, 15] = 2;
            matrix[45, 21] = 2;
            matrix[40, 23] = 2;
            matrix[40, 17] = 2;

            matrix[30, 34] = 2;
            matrix[31, 32] = 2;
            matrix[20, 20] = 2;
            matrix[35, 39] = 2;
            matrix[37, 37] = 2;
            matrix[50, 50] = 2;
            matrix[60, 60] = 2;
            matrix[57, 57] = 2;
            matrix[47, 51] = 2;
            matrix[50, 55] = 2;
            matrix[60, 50] = 2;
            matrix[53, 57] = 2;
            matrix[60, 63] = 2;
            matrix[48, 49] = 2;
            matrix[51, 51] = 2;
            matrix[47, 57] = 2;

            return matrix;
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
            binarySynapticWeights = new BinarySynapticWeightsAlgorithm();

            var trainingSamples = Preprocessing.GetTrainingSamplesFromMatrix(matrix);

            binarySynapticWeights.Train(trainingSamples);

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
                        var predictedClass = binarySynapticWeights.PredictClass(inputVector);

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
