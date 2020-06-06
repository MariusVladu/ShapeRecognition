using BinarySynapticWeights.Nodes;
using BinarySynapticWeights.SynapticLinks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class NeuralNetworkTrainingAlgorithm
    {
        private static readonly HammingDistance HammingDistance = new HammingDistance();
        private static readonly Vectors Vectors = new Vectors();

        private BSWModel model;

        private List<Sample> trainingSamples;

        private List<Sample> processedSamples;
        private List<Sample> trainingSamplesNotProcessed;
        private List<Sample> enclosedSamplesOfOtherClasses;
        private ILookup<int, Sample> samplesLookupByHammingDistanceFromKey;


        public BSWModel Train(List<Sample> trainingSamples)
        {
            this.trainingSamples = trainingSamples;
            InitializeNodes();

            foreach (var outputClass in trainingSamples.Select(x => x.OutputClass).Distinct())
            {
                BuildModelToRecognizeClass(outputClass);
            }

            return model;
        }

        private void InitializeNodes()
        {
            model = new BSWModel
            {
                InputNodes = new List<InputNode>(),
                HiddenNodes = new List<HiddenNode>(),
                OutputNodes = new List<OutputNode>(),

                InputToHiddenSynapticLinks = new List<InputToHiddenSynapticLink>(),
                HiddenToOutputSynapticLinks = new List<HiddenToOutputSynapticLink>()
            };

            var inputLayerNodesCount = trainingSamples[0].InputVector.Count;

            for (int i = 0; i < inputLayerNodesCount; i++)
            {
                model.InputNodes.Add(new InputNode());
            }
        }

        private void BuildModelToRecognizeClass(string outputClass)
        {
            trainingSamplesNotProcessed = new List<Sample>(trainingSamples);
            enclosedSamplesOfOtherClasses = new List<Sample>();

            var outputNode = new OutputNode { Class = outputClass };
            var maximumNumberOfPlanesUsedForAPattern = 0;

            while (AreTrainingSamplesOfThisClassNotProcessed(outputClass))
            {
                var numberOfPlanesUsedForThisPattern = 0;
                processedSamples = new List<Sample>();

                var averageVector = Vectors.GetAverageVector(outputClass, trainingSamplesNotProcessed);
                var key = Vectors.GetKeySample(averageVector, outputClass, trainingSamplesNotProcessed);
                var yes = Vectors.GetYesSample(key, trainingSamplesNotProcessed);
                var no = Vectors.GetNoSample(key, trainingSamplesNotProcessed);

                var distance = GetFirstSeparationDistance(key, yes);
                CreateSeparationPlane(distance, key, outputNode);
                numberOfPlanesUsedForThisPattern++;

                while (enclosedSamplesOfOtherClasses.Any())
                {
                    key = enclosedSamplesOfOtherClasses.First();
                    samplesLookupByHammingDistanceFromKey = trainingSamplesNotProcessed.ToLookup(x => HammingDistance.GetHammingDistance(key, x));

                    distance = GetSeparationDistanceForOtherClasses(outputClass);
                    CreateSeparationPlane(distance, key, outputNode);
                    numberOfPlanesUsedForThisPattern++;

                    enclosedSamplesOfOtherClasses.Remove(key);
                }

                if (numberOfPlanesUsedForThisPattern > maximumNumberOfPlanesUsedForAPattern)
                {
                    maximumNumberOfPlanesUsedForAPattern = numberOfPlanesUsedForThisPattern;
                }

                UpdateTrainingSamplesNotProcessed();
            }

            outputNode.Threshold = maximumNumberOfPlanesUsedForAPattern - 0.5;
            model.OutputNodes.Add(outputNode);
        }

        private int GetFirstSeparationDistance(Sample key, Sample yes)
        {
            samplesLookupByHammingDistanceFromKey = trainingSamplesNotProcessed.ToLookup(x => HammingDistance.GetHammingDistance(key, x));
            var distance = 0;

            while (distance < HammingDistance.GetHammingDistance(key, yes))
            {
                var samplesOfThisClassCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass == key.OutputClass);
                var samplesOfOtherClassesCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass != key.OutputClass);

                if (samplesOfThisClassCount < samplesOfOtherClassesCount)
                {
                    break;
                }
                else
                {
                    distance++;
                }
            }

            return distance;
        }

        private int GetSeparationDistanceForOtherClasses(string outputClass)
        {
            int samplesOfThisClassCount, samplesOfOtherClassesCount;
            var distance = 0;
            do
            {
                distance++;
                samplesOfThisClassCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass == outputClass);
                samplesOfOtherClassesCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass != outputClass);
            }
            while (samplesOfThisClassCount <= samplesOfOtherClassesCount);

            return distance;
        }

        private void CreateSeparationPlane(int distance, Sample key, OutputNode outputNode)
        {
            distance = Math.Max(distance - 1, 0);

            CreateSeparationPlaneNodesAndLinks(distance, key, outputNode);

            EncloseAndMarkAsProcessedSamplesCloserThanDistance(distance, outputNode.Class);
        }

        private void CreateSeparationPlaneNodesAndLinks(int distance, Sample key, OutputNode outputNode)
        {
            var threshold = distance + 0.5 - key.InputVector.Count(x => x == 1);

            var hiddenNode = new HiddenNode { Threshold = -threshold };
            model.HiddenNodes.Add(hiddenNode);

            for (int i = 0; i < key.InputVector.Count; i++)
            {
                model.InputToHiddenSynapticLinks.Add(new InputToHiddenSynapticLink
                {
                    InputNode = model.InputNodes[i],
                    HiddenNode = hiddenNode,
                    Weight = key.InputVector[i] == 1 ? 1 : -1
                });
            }

            model.HiddenToOutputSynapticLinks.Add(new HiddenToOutputSynapticLink
            {
                HiddenNode = hiddenNode,
                OutputNode = outputNode
            });
        }

        private void EncloseAndMarkAsProcessedSamplesCloserThanDistance(int distance, string outputClass)
        {
            for (int i = 0; i <= distance; i++)
            {
                foreach (var sample in samplesLookupByHammingDistanceFromKey[i])
                {
                    if (sample.OutputClass != outputClass && !enclosedSamplesOfOtherClasses.Contains(sample))
                    {
                        enclosedSamplesOfOtherClasses.Add(sample);
                    }

                    processedSamples.Add(sample);
                }
            }
        }

        private bool AreTrainingSamplesOfThisClassNotProcessed(string outputClass)
        {
            return trainingSamplesNotProcessed.Any(x => x.OutputClass == outputClass);
        }

        private void UpdateTrainingSamplesNotProcessed()
        {
            foreach (var sample in processedSamples)
            {
                trainingSamplesNotProcessed.Remove(sample);
            }
        }
    }
}
