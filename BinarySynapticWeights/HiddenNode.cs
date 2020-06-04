﻿namespace BinarySynapticWeights
{
    public class HiddenNode
    {
        public double Threshold { get; set; }
        public int Value { get; set; }
        public bool IsActivated { get; set; }

        public void ApplyActivationFunction()
        {
            if (Value >= Threshold)
            {
                IsActivated = true;
                Value = 1;
            }
            else
            {
                IsActivated = false;
                Value = 0;
            }
        }
    }
}