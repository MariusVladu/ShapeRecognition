using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BinarySynapticWeights.Tests
{
    [TestClass]
    public class BinarySynapticWeightsTests
    {
        private BinarySynapticWeights binarySynapticWeights;

        [TestInitialize]
        public void Setup()
        {
            binarySynapticWeights = new BinarySynapticWeights();
        }

        [TestMethod]
        public void TestMethod1()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(new List<int> { 0, 1, 1 });
        }
    }
}
