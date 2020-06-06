namespace BinarySynapticWeightsPointsApplication
{
    public static class TrainingSets
    {
        public static int[,] GetFirstTrainingSet(int matrixSize)
        {
            var matrix = new int[matrixSize, matrixSize];

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

        public static int[,] GetSecondTrainingSet(int matrixSize)
        {
            var matrix = new int[matrixSize, matrixSize];

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

        public static int[,] GetThirdTrainingSet(int matrixSize)
        {
            var matrix = new int[matrixSize, matrixSize];

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
    }
}
