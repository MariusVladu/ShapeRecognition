using System.Collections.Generic;

namespace BinarySynapticWeights.Contracts
{
    public interface IBSWNeuralNetwork
    {
        string PredictClass(List<int> inputVector);
    }
}
