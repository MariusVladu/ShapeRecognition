using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class BinarySynapticWeights
    {
        private readonly List<InputNode> inputNodes = new List<InputNode>();
        private readonly List<HiddenNode> hiddenNodes = new List<HiddenNode>();
        private readonly List<OutputNode> outputNodes = new List<OutputNode>();

        private readonly List<InputToHiddenSynapticLink> inputToHiddenSynapticLinks = new List<InputToHiddenSynapticLink>();
        private readonly List<HiddenToOutputSynapticLink> hiddenToOutputSynapticLinks = new List<HiddenToOutputSynapticLink>();

        private List<Sample> trainingSamples;

        public string PredictClass(List<int> inputVector)
        {
            if (outputNodes.Count == 0)
            {
                throw new InvalidOperationException("Binary Synaptic Weights neural network is not trained");
            }

            if (inputVector.Count != inputNodes.Count)
            {
                throw new ArgumentException($"{nameof(inputVector)} should have the same size as the samples used to train the network: {inputNodes.Count}");
            }

            ApplyVectorOnInputNodes(inputVector);
            AddWeightsToHiddenNodes();
            ApplyActivationFunctionOnHiddenNodes();
            AddWeightsToOutputNodes();
            ApplyActivationFunctionOnOutputNodes();

            return GetActivatedOutputNodeClass();
        }

        private void AddWeightsToHiddenNodes()
        {
            foreach (var link in inputToHiddenSynapticLinks)
            {
                link.AddWeightToHiddenNode();
            }
        }

        private void ApplyActivationFunctionOnHiddenNodes()
        {
            foreach (var hiddenNode in hiddenNodes)
            {
                hiddenNode.ApplyActivationFunction();
            }
        }

        private void AddWeightsToOutputNodes()
        {
            foreach (var link in hiddenToOutputSynapticLinks)
            {
                link.AddWeightToOutputNode();
            }
        }

        private void ApplyActivationFunctionOnOutputNodes()
        {
            foreach (var outputNode in outputNodes)
            {
                outputNode.ApplyActivationFunction();
            }
        }

        private string GetActivatedOutputNodeClass()
        {
            var activatedOutputNode = outputNodes.FirstOrDefault(x => x.IsActivated);
            if(activatedOutputNode == null)
            {
                throw new Exception("No output node was activated");
            }

            return activatedOutputNode.Class;
        }

        private void ApplyVectorOnInputNodes(List<int> inputVector)
        {
            for (int i = 0; i < inputNodes.Count; i++)
            {
                inputNodes[i].Value = inputVector[i];
            }
        }

        public void Train(List<Sample> trainingSamples)
        {
            this.trainingSamples = trainingSamples;
            InitializeNodes();

            foreach (var outputClass in GetDistinctOutputClasses())
            {
                BuildModelToRecognizeClass(outputClass);
            }
        }

        private void InitializeNodes()
        {
            var inputLayerNodesCount = trainingSamples[0].InputVector.Count;

            for (int i = 0; i < inputLayerNodesCount; i++)
            {
                inputNodes.Add(new InputNode());
            }
        }

        private List<string> GetDistinctOutputClasses()
        {
            return trainingSamples.Select(x => x.OutputClass).Distinct().ToList();
        }

        private void BuildModelToRecognizeClass(string outputClass)
        {
            var enclosedSamples = new List<Sample>();
            var outputNode = new OutputNode
            {
                Class = outputClass
            };

            var maximumNumberOfPlanesUsedForAPattern = -1;

            while (!AreAllSamplesOfThisClassEnclosed(outputClass, enclosedSamples))
            {
                var numberOfPlanesUsedForThisPattern = 0;

                var samplesNotEnclosed = trainingSamples.Where(x => !enclosedSamples.Contains(x)).ToList();
                var samplesOfThisClassNotEnclosed = samplesNotEnclosed.Where(x => x.OutputClass == outputClass).ToList();
                var samplesNotOfThisClassNotEnclosed = samplesNotEnclosed.Where(x => x.OutputClass != outputClass).ToList();

                var averageVector = GetAverageVector(samplesOfThisClassNotEnclosed);
                var key = GetKeySample(averageVector, samplesOfThisClassNotEnclosed);
                var yes = GetYesSample(key, samplesOfThisClassNotEnclosed);
                var no = GetNoSample(key, samplesNotOfThisClassNotEnclosed);

                var distance = 0;
                var samplesLookupByHammingDistanceFromKey = samplesNotEnclosed.ToLookup(x => GetHammingDistance(key, x));

                if (GetHammingDistance(key, yes) >= GetHammingDistance(key, no))
                {
                    distance = 1;
                    while(distance < GetHammingDistance(key, yes))
                    {
                        var samplesOfThisClassCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass == outputClass);
                        var samplesOfOtherClassesCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass != outputClass);

                        if(samplesOfThisClassCount < samplesOfOtherClassesCount)
                        {
                            break;
                        }
                        else
                        {
                            distance++;
                        }
                    }
                }

                distance = Math.Max(distance - 1, 0);
                CreateSeparationPlane(distance, key, outputNode);
                EncloseSamplesCloserThanDistance(distance, enclosedSamples, samplesLookupByHammingDistanceFromKey);
                numberOfPlanesUsedForThisPattern++;

                while (ThereAreEnclosedSamplesNotOfThisClass(enclosedSamples, outputClass))
                {
                    key = enclosedSamples.First(x => x.OutputClass != outputClass);

                    samplesLookupByHammingDistanceFromKey = samplesNotEnclosed.ToLookup(x => GetHammingDistance(key, x));

                    int samplesOfThisClassCount, samplesOfOtherClassesCount;
                    distance = 0;
                    do
                    {
                        distance++;
                        samplesOfThisClassCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass == outputClass);
                        samplesOfOtherClassesCount = samplesLookupByHammingDistanceFromKey[distance].Count(x => x.OutputClass != outputClass);
                    }
                    while (samplesOfThisClassCount <= samplesOfOtherClassesCount);

                    distance = Math.Max(distance - 1, 0);
                    CreateSeparationPlane(distance, key, outputNode);
                    EncloseSamplesCloserThanDistance(distance, enclosedSamples, samplesLookupByHammingDistanceFromKey);
                    numberOfPlanesUsedForThisPattern++;
                }

                if (numberOfPlanesUsedForThisPattern > maximumNumberOfPlanesUsedForAPattern)
                {
                    maximumNumberOfPlanesUsedForAPattern = numberOfPlanesUsedForThisPattern;
                }
            }

            if (maximumNumberOfPlanesUsedForAPattern > 0)
            {
                outputNode.Threshold = maximumNumberOfPlanesUsedForAPattern - 0.5;
                outputNodes.Add(outputNode);
            }
        }

        private double CalculateThresholdForOutputNode(OutputNode outputNode)
        {
            return hiddenToOutputSynapticLinks.Count(x => x.OutputNode == outputNode) - 0.5;
        }

        private bool ThereAreEnclosedSamplesNotOfThisClass(List<Sample> enclosedSamples, string outputClass)
        {
            return enclosedSamples.Any(x => x.OutputClass != outputClass);
        }

        private void CreateSeparationPlane(int distance, Sample key, OutputNode outputNode)
        {
            var threshold = distance + 0.5 - key.InputVector.Count(x => x == 1);

            var hiddenNode = new HiddenNode { Threshold = -threshold };
            hiddenNodes.Add(hiddenNode);

            for(int i = 0; i < key.InputVector.Count; i++)
            {
                inputToHiddenSynapticLinks.Add(new InputToHiddenSynapticLink
                {
                    InputNode = inputNodes[i],
                    HiddenNode = hiddenNode,
                    Weight = key.InputVector[i] == 1 ? 1 : -1
                });
            }

            hiddenToOutputSynapticLinks.Add(new HiddenToOutputSynapticLink
            {
                HiddenNode = hiddenNode,
                OutputNode = outputNode
            });
        }

        private void EncloseSamplesCloserThanDistance(int distance, List<Sample> enclosedSamples, ILookup<int, Sample> samplesLookupByHammingDistanceFromKey)
        {
            for (int i = 0; i <= distance; i++)
            {
                foreach (var sample in samplesLookupByHammingDistanceFromKey[i])
                {
                    enclosedSamples.Add(sample);
                }
            }
        }

        private Dictionary<int, List<Sample>> GetSamplesGroupedByTheirHammingDistanceFromTheKey(List<Sample> samplesNotEnclosed, List<int> key)
        {
            return samplesNotEnclosed
                .GroupBy(x => GetHammingDistance(key, x.InputVector))
                .ToDictionary(group => group.Key, group => group.ToList());
        }

        private bool AreAllSamplesOfThisClassEnclosed(string outputClass, List<Sample> enclosedSamples)
        {
            return !trainingSamples.Any(x => x.OutputClass == outputClass && !enclosedSamples.Contains(x));
        }

        private List<int> GetAverageVector(List<Sample> trainingSamplesOfThisClassNotEnclosed)
        {
            var averageVector = new List<int>();

            for (int i = 0; i < inputNodes.Count; i++)
            {
                double sum = 0;
                foreach (var sample in trainingSamplesOfThisClassNotEnclosed)
                {
                    sum += sample.InputVector[i];
                }

                averageVector.Add((int) Math.Round(sum / trainingSamplesOfThisClassNotEnclosed.Count()));
            }

            return averageVector;
        }

        private Sample GetKeySample(List<int> averageVector, List<Sample> trainingSamplesOfThisClassNotEnclosed)
        {
            var closestSample = trainingSamplesOfThisClassNotEnclosed[0];
            var minimumHammingDistance = int.MaxValue;

            foreach (var sample in trainingSamplesOfThisClassNotEnclosed)
            {
                var hammingDistance = GetHammingDistance(averageVector, sample.InputVector);

                if(hammingDistance < minimumHammingDistance)
                {
                    closestSample = sample;
                    minimumHammingDistance = hammingDistance;
                }
            }

            return closestSample;
        }

        private Sample GetYesSample(Sample key, List<Sample> trainingSamplesOfThisClassNotEnclosed)
        {
            var furthestSample = trainingSamplesOfThisClassNotEnclosed[0];
            var maximumHammingDistance = int.MinValue;

            foreach (var sample in trainingSamplesOfThisClassNotEnclosed)
            {
                var hammingDistance = GetHammingDistance(key, sample);

                if (hammingDistance > maximumHammingDistance)
                {
                    furthestSample = sample;
                    maximumHammingDistance = hammingDistance;
                }
            }

            return furthestSample;
        }

        private Sample GetNoSample(Sample key, List<Sample> trainingSamplesOfOtherClassesNotEnclosed)
        {
            var closestSample = trainingSamplesOfOtherClassesNotEnclosed[0];
            var minimumHammingDistance = int.MaxValue;

            foreach (var sample in trainingSamplesOfOtherClassesNotEnclosed)
            {
                var hammingDistance = GetHammingDistance(key, sample);

                if (hammingDistance < minimumHammingDistance)
                {
                    closestSample = sample;
                    minimumHammingDistance = hammingDistance;
                }
            }

            return closestSample;
        }

        private int GetHammingDistance(Sample sample1, Sample sample2)
        {
            return GetHammingDistance(sample1.InputVector, sample2.InputVector);
        }

        private int GetHammingDistance(List<int> vector1, List<int> vector2)
        {
            if(vector1.Count != vector2.Count)
            {
                throw new InvalidOperationException("Both vectors must have the same length in order to calculate the Hamming distance between them");
            }

            int differences = 0;
            for (int i = 0; i < vector1.Count; i++)
            {
                if(vector1[i] != vector2[i])
                {
                    differences++;
                }
            }

            return differences;
        }
    }
}
