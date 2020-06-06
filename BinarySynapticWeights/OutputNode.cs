namespace BinarySynapticWeights
{
    public class OutputNode
    {
        public string Class { get; set; }
        public double Threshold { get; set; }
        public int Value { get; set; }
        public bool IsActivated { get; set; }

        public void ApplyActivationFunction()
        {
            if (Value >= Threshold)
            {
                IsActivated = true;
            }
            else
            {
                IsActivated = false;
            }
        }

        public void Reset()
        {
            Value = 0;
            IsActivated = false;
        }
    }
}
