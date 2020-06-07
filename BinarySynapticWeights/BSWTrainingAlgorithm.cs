using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.Entities;
using BinarySynapticWeights.Nodes;
using BinarySynapticWeights.SynapticLinks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class BSWTrainingAlgorithm : IBSWTrainingAlgorithm
    {
        private static readonly HammingDistance HammingDistance = new HammingDistance();
        private static readonly Vectors Vectors = new Vectors();

        private Model model;

        private List<Sample> trainingSamples;

        private List<Sample> processedSamples;
        private List<Sample> trainingSamplesNotProcessed;
        private List<Sample> enclosedSamplesOfOtherClasses;
        private ILookup<int, Sample> samplesLookupByHammingDistanceFromKey;

        public Model Train(List<Sample> trainingSamples)
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
            model = new Model
            {
                InputNodes = new List<InputNode>(),
                HiddenNodes = new List<HiddenNode>(),
                OutputNodes = new List<OutputNode>(),

                InputToHiddenSynapticLinks = new List<InputToHiddenSynapticLink>(),
                HiddenToOutputSynapticLinks = new List<HiddenToOutputSynapticLink>()
            };

            var inputLayerNodesCount = trainingSamples[0].InputVector.Count;

            for (int i = 0; i < inputLayerNodesCount; i++)
                model.InputNodes.Add(new InputNode());
        }

        private void BuildModelToRecognizeClass(string outputClass)
        {
            trainingSamplesNotProcessed = new List<Sample>(trainingSamples);
            enclosedSamplesOfOtherClasses = new List<Sample>();

            var outputNode = new OutputNode { Class = outputClass };
            double numberOfPatterns = 0;

            while (AreTrainingSamplesOfThisClassNotProcessed(outputClass))
            {
                numberOfPatterns++;
                processedSamples = new List<Sample>();

                var averageVector = Vectors.GetAverageVector(outputClass, trainingSamplesNotProcessed);
                var key = Vectors.GetKeySample(averageVector, outputClass, trainingSamplesNotProcessed);
                var yes = Vectors.GetYesSample(key, trainingSamplesNotProcessed);
                var no = Vectors.GetNoSample(key, trainingSamplesNotProcessed);

                var distance = GetFirstSeparationDistance(key, yes, no);
                CreateSeparationPlane(distance, key, outputNode);

                while (enclosedSamplesOfOtherClasses.Any())
                {
                    key = enclosedSamplesOfOtherClasses.First();
                    samplesLookupByHammingDistanceFromKey = trainingSamplesNotProcessed.ToLookup(x => HammingDistance.GetHammingDistance(key, x));

                    distance = GetSeparationDistanceForOtherClasses(outputClass);
                    CreateSeparationPlane(distance, key, outputNode);

                    enclosedSamplesOfOtherClasses.Remove(key);
                }

                UpdateTrainingSamplesNotProcessed();
            }

            outputNode.Threshold = Math.Round(model.HiddenToOutputSynapticLinks.Count(x => x.OutputNode == outputNode) / numberOfPatterns) - 0.5;
            model.OutputNodes.Add(outputNode);
        }

        private int GetFirstSeparationDistance(Sample key, Sample yes, Sample no)
        { 
            samplesLookupByHammingDistanceFromKey = trainingSamplesNotProcessed.ToLookup(x => HammingDistance.GetHammingDistance(key, x));

            int maxSearchDistance = HammingDistance.GetHammingDistance(key, yes);

            int samplesOfThisClassCount;
            int samplesOfOtherClassesCount;
            var distance = 0;

            do
            {
                distance++;
                samplesOfThisClassCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass == key.OutputClass);
                samplesOfOtherClassesCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass != key.OutputClass);
            }
            while (samplesOfThisClassCount >= samplesOfOtherClassesCount && distance <= maxSearchDistance);

            return distance;
        }

        private int GetSeparationDistanceForOtherClasses(string outputClass)
        {
            int samplesOfThisClassCount;
            int samplesOfOtherClassesCount;
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
            CreateSeparationPlaneNodesAndLinks(distance - 1, key, outputNode);

            EncloseAndMarkAsProcessedSamplesCloserThanDistance(distance - 1, outputNode.Class);
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
