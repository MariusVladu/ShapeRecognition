using BinarySynapticWeights.Entities;
using System.Collections.Generic;

namespace BinarySynapticWeights.Contracts
{
    public interface IBSWTrainingAlgorithm
    {
        Model Train(List<Sample> trainingSamples);
    }
}
