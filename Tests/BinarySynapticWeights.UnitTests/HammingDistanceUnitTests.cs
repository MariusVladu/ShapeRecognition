using BinarySynapticWeights.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BinarySynapticWeights.UnitTests
{
    [TestClass]
    public class HammingDistanceUnitTests
    {
        private HammingDistance hammingDistance;

        [TestInitialize]
        public void Setup()
        {
            hammingDistance = new HammingDistance();
        }

        [TestMethod]
        public void TestThatWhenThereIsNoDifferenceBetweenVectorsGetHammingDistanceReturns0()
        {
            var vector1 = new List<int> { 0, 0, 1, 0, 1, 1, 0 };
            var vector2 = new List<int> { 0, 0, 1, 0, 1, 1, 0 };

            var result = hammingDistance.GetHammingDistance(vector1, vector2);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestThatWhenThereIs1DifferenceBetweenVectorsGetHammingDistanceReturns1()
        {
            var vector1 = new List<int> { 0, 0, 1, 0, 1, 1, 0 };
            var vector2 = new List<int> { 0, 1, 1, 0, 1, 1, 0 };

            var result = hammingDistance.GetHammingDistance(vector1, vector2);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void TestThatWhenThereAre2DifferencesBetweenVectorsGetHammingDistanceReturns2()
        {
            var vector1 = new List<int> { 0, 0, 1, 0, 1, 1, 0 };
            var vector2 = new List<int> { 1, 0, 1, 0, 1, 1, 1 };

            var result = hammingDistance.GetHammingDistance(vector1, vector2);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void TestThatWhenAllElementsAreDifferentGetHammingDistanceReturnsExpectedDistance()
        {
            var vector1 = new List<int> { 0, 0, 1, 0, 1, 1, 0 };
            var vector2 = new List<int> { 1, 1, 0, 1, 0, 0, 1 };
            var expectedDistance = vector1.Count;

            var result = hammingDistance.GetHammingDistance(vector1, vector2);

            Assert.AreEqual(expectedDistance, result);
        }

        [TestMethod]
        public void TestThatGetHammingDistanceReturnsExpectedDistanceForSamples()
        {
            var sample1 = new Sample { InputVector = new List<int> { 0, 0, 1, 0, 1, 1, 0 } };
            var sample2 = new Sample { InputVector = new List<int> { 0, 0, 0, 1, 0, 0, 1 } };
            var expectedDistance = 5;

            var result = hammingDistance.GetHammingDistance(sample1, sample2);

            Assert.AreEqual(expectedDistance, result);
        }
    }
}
