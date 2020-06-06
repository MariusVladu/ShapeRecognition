using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BinarySynapticWeights.UnitTests
{
    [TestClass]
    public class VectorsUnitTests
    {
        private Vectors vectors;

        [TestInitialize]
        public void Setup()
        {
            vectors = new Vectors();
        }

        [TestMethod]
        public void TestThatWhenAverageVectorExistsAmongSamplesGetKeySampleReturnsExactVector()
        {
            var samplesList = new List<Sample>
            {
                new Sample { InputVector = new List<int> { 0, 0, 1, 1, 1, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 0, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 0, 1, 0, 1, 0, 1 }, OutputClass = "class1" },
            };
            var averageVector = new List<int> { 1, 0, 1, 0, 1, 0, 1 };

            var result = vectors.GetKeySample(averageVector, "class1", samplesList);

            CollectionAssert.AreEqual(samplesList[2].InputVector, result.InputVector);
        }

        [TestMethod]
        public void TestThatWhenAverageVectorDoesNotExistAmongSamplesGetKeySampleReturnsTheClosestVector()
        {
            var samplesList = new List<Sample>
            {
                new Sample { InputVector = new List<int> { 0, 0, 1, 1, 1, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 0, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 0, 1, 0, 1, 0, 1 }, OutputClass = "class1" },
            };
            var averageVector = new List<int> { 1, 1, 1, 0, 1, 0, 1 };

            var result = vectors.GetKeySample(averageVector, "class1", samplesList);

            CollectionAssert.AreEqual(samplesList[2].InputVector, result.InputVector);
        }

        [TestMethod]
        public void TestThatGetYesSampleReturnsTheFurthestSampleOfTheSameClass()
        {
            var samplesList = new List<Sample>
            {
                new Sample { InputVector = new List<int> { 0, 0, 0, 0, 1, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 0, 0, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 0, 1, 1, 1, 1, 1, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 1, 1, 1 }, OutputClass = "anotherClass" }
            };
            var key = new Sample { InputVector = new List<int> { 1, 1, 1, 1, 1, 1, 1 }, OutputClass = "class1" };

            var result = vectors.GetYesSample(key, samplesList);

            Assert.AreEqual(samplesList[0], result);
        }

        [TestMethod]
        public void TestThatGetNoSampleReturnsTheClosestSampleOfTheAnotherClass()
        {
            var samplesList = new List<Sample>
            {
                new Sample { InputVector = new List<int> { 0, 0, 0, 0, 1, 0, 1 }, OutputClass = "anotherClass" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 0, 0, 1 }, OutputClass = "anotherClass" },
                new Sample { InputVector = new List<int> { 0, 1, 1, 1, 1, 1, 1 }, OutputClass = "class1" },
                new Sample { InputVector = new List<int> { 1, 1, 1, 1, 1, 1, 1 }, OutputClass = "class1" }
            };
            var key = new Sample { InputVector = new List<int> { 1, 1, 1, 1, 1, 1, 1 }, OutputClass = "class1" };

            var result = vectors.GetNoSample(key, samplesList);

            Assert.AreEqual(samplesList[1], result);
        }
    }
}
