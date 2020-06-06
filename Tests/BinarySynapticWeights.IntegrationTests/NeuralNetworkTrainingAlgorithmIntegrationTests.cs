using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights.IntegrationTests
{
    [TestClass]
    public class NeuralNetworkTrainingAlgorithmIntegrationTests
    {
        private NeuralNetworkTrainingAlgorithm algorithm;

        private BSWModel trainedModel;

        [TestInitialize]
        public void Setup()
        {
            algorithm = new NeuralNetworkTrainingAlgorithm();

            trainedModel = algorithm.Train(TestEntities.GetTrainingSamples());
        }

        [Ignore("Wrong example in pdf ?")]
        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP1()
        {
            var result = trainedModel.PredictClass(TestEntities.GetSample1().InputVector);

            Assert.AreEqual("black", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP2()
        {
            var result = trainedModel.PredictClass(TestEntities.GetSample2().InputVector);

            Assert.AreEqual("black", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP3()
        {
            var result = trainedModel.PredictClass(TestEntities.GetSample3().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP4()
        {
            var result = trainedModel.PredictClass(TestEntities.GetSample4().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkCorrectlyRecognisesP5()
        {
            var result = trainedModel.PredictClass(TestEntities.GetSample5().InputVector);

            Assert.AreEqual("white", result);
        }

        [TestMethod]
        public void TestThatNetworkTrainedWith2GroupsOfNumbersCanRecognizeNumberFromTheFirstGroup()
        {
            string expectedClass = "first group";
            TrainNetworkWith2GroupsOfNumbers(expectedClass, "second group");
            var valueFromGroup1 = SerialCoding.GetSeriallyCodedValue(15, 100);

            var result = trainedModel.PredictClass(valueFromGroup1);

            Assert.AreEqual(expectedClass, result);
        }

        [TestMethod]
        public void TestThatNetworkTrainedWith2GroupsOfNumbersCanRecognizeNumberFromTheSecondGroup()
        {
            var expectedClass = "second group";
            TrainNetworkWith2GroupsOfNumbers("first group", expectedClass);
            var valueFromGroup1 = SerialCoding.GetSeriallyCodedValue(85, 100);

            var result = trainedModel.PredictClass(valueFromGroup1);

            Assert.AreEqual(expectedClass, result);
        }

        private void TrainNetworkWith2GroupsOfNumbers(string class1, string class2)
        {
            var group1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 50 };
            var group2 = new List<int> { 60, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };

            var group1Samples = group1.Select(x => new Sample { InputVector = SerialCoding.GetSeriallyCodedValue(x, 100), OutputClass = class1 });
            var group2Samples = group2.Select(x => new Sample { InputVector = SerialCoding.GetSeriallyCodedValue(x, 100), OutputClass = class2 });

            var allSamples = group1Samples.Concat(group2Samples).ToList();

            trainedModel = algorithm.Train(allSamples);
        }
    }
}
