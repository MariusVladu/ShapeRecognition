using BinarySynapticWeights;
using BinarySynapticWeights.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeightsPointsApplication
{
    public static class Preprocessing
    {
        public static List<int> GetInputVectorFromCoordinates(int x, int y)
        {
            var xSerialCode = SerialCoding.GetSeriallyCodedValue(x, 64);
            var ySerialCode = SerialCoding.GetSeriallyCodedValue(y, 64);

            return xSerialCode.Concat(ySerialCode).ToList();
        }

        public static List<Sample> GetTrainingSamplesFromMatrix(int[,] matrix)
        {
            var samples = new List<Sample>();

            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        samples.Add(new Sample
                        {
                            InputVector = GetInputVectorFromCoordinates(i, j),
                            OutputClass = "first group of pixels"
                        });
                    }

                    else if (matrix[i, j] == 2)
                    {
                        samples.Add(new Sample
                        {
                            InputVector = GetInputVectorFromCoordinates(i, j),
                            OutputClass = "second group of pixels"
                        });
                    }

                    else if (matrix[i, j] == 3)
                    {
                        samples.Add(new Sample
                        {
                            InputVector = GetInputVectorFromCoordinates(i, j),
                            OutputClass = "third group of pixels"
                        });
                    }
                }
            }

            return samples;
        }
    }
}
