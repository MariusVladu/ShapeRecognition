using System;
using System.Collections.Generic;

namespace BinarySynapticWeights
{
    public class HammingDistance
    {
        public int GetHammingDistance(Sample sample1, Sample sample2)
        {
            return GetHammingDistance(sample1.InputVector, sample2.InputVector);
        }

        public int GetHammingDistance(List<int> vector1, List<int> vector2)
        {
            if (vector1.Count != vector2.Count)
            {
                throw new InvalidOperationException("Both vectors must have the same length in order to calculate the Hamming distance between them");
            }

            int differences = 0;
            for (int i = 0; i < vector1.Count; i++)
            {
                if (vector1[i] != vector2[i])
                {
                    differences++;
                }
            }

            return differences;
        }
    }
}
