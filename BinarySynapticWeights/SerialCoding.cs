using System;
using System.Collections.Generic;

namespace BinarySynapticWeights
{
    public static class SerialCoding
    {
        public static List<int> GetSeriallyCodedValue(int value, int length)
        {
            if (value < 0)
            {
                throw new ArgumentException($"Negative values not supported");
            }

            if (value > length)
            {
                throw new ArgumentException($"Value {value} cannot be coded on length {length}");
            }

            var serialCode = new List<int>();

            var numberOf1s = value;
            var numberOf0s = length - numberOf1s;

            for (int i = 0; i < numberOf0s; i++)
                serialCode.Add(0);

            for (int i = 0; i < numberOf1s; i++)
                serialCode.Add(1);

            return serialCode;
        }
    }
}
