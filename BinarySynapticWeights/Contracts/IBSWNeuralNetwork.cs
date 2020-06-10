using BinarySynapticWeights.Entities;
using System.Collections.Generic;

namespace BinarySynapticWeights.Contracts
{
    public interface IBSWNeuralNetwork
    {
        Model Model { get; set; }
        string PredictClass(List<int> inputVector);
    }
}
