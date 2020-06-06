using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights.Tests
{
    [TestClass]
    public class BinarySynapticWeightsTests
    {
        private BinarySynapticWeightsAlgorithm binarySynapticWeights;

        [TestInitialize]
        public void Setup()
        {
            binarySynapticWeights = new BinarySynapticWeightsAlgorithm();
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesTrainedSamples()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            foreach (var sample in TestEntities.GetTrainingSamples())
            {
                var predictedClass = binarySynapticWeights.PredictClass(sample.InputVector);

                Assert.AreEqual(sample.OutputClass, predictedClass);
            }
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP1()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());
            
            var result = binarySynapticWeights.PredictClass(TestEntities.GetSample1().InputVector);

            Assert.AreEqual("black", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP2()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(TestEntities.GetSample2().InputVector);

            Assert.AreEqual("black", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP3()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(TestEntities.GetSample3().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP4()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(TestEntities.GetSample4().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP5()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(TestEntities.GetSample5().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesNewWhitePoint1()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(new List<int> { 0, 1, 1});

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesNewWhitePoint2()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(new List<int> { 1, 0, 1 });

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesNewWhitePoint3()
        {
            binarySynapticWeights.Train(TestEntities.GetTrainingSamples());

            var result = binarySynapticWeights.PredictClass(new List<int> { 0, 1, 0 });

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkTrainedWith2GroupsOfNumbersCanRecognizeNumberFromTheFirstGroup()
        {
            var group1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 50 };
            var group2 = new List<int> { 60, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };

            var group1Samples = group1.Select(x => new Sample { InputVector = SerialCoding.GetSeriallyCodedValue(x, 100), OutputClass = "first group" });
            var group2Samples = group2.Select(x => new Sample { InputVector = SerialCoding.GetSeriallyCodedValue(x, 100), OutputClass = "second group" });

            var allSamples = group1Samples.Concat(group2Samples).ToList();

            var valueFromGroup1 = SerialCoding.GetSeriallyCodedValue(83, 100);

            binarySynapticWeights.Train(allSamples);

            var result = binarySynapticWeights.PredictClass(valueFromGroup1);
        }
    }
}
