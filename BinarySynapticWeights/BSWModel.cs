using BinarySynapticWeights.Nodes;
using BinarySynapticWeights.SynapticLinks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class BSWModel
    {
        public List<InputNode> InputNodes { get; set; }
        public List<HiddenNode> HiddenNodes { get; set; }
        public List<OutputNode> OutputNodes { get; set; }

        public List<InputToHiddenSynapticLink> InputToHiddenSynapticLinks { get; set; }
        public List<HiddenToOutputSynapticLink> HiddenToOutputSynapticLinks { get; set; }

        public string PredictClass(List<int> inputVector)
        {
            if (OutputNodes.Count == 0)
            {
                throw new InvalidOperationException("Binary Synaptic Weights neural network is not trained");
            }

            if (inputVector.Count != InputNodes.Count)
            {
                throw new ArgumentException($"{nameof(inputVector)} should have the same size as the samples used to train the network: {InputNodes.Count}");
            }

            ResetNodeValues();

            ApplyVectorOnInputNodes(inputVector);
            AddWeightsToHiddenNodes();
            ApplyActivationFunctionOnHiddenNodes();
            AddWeightsToOutputNodes();
            ApplyActivationFunctionOnOutputNodes();

            return GetTheMostActivatedOutputNodeClass();
        }

        private void ResetNodeValues()
        {
            foreach (var hiddenNode in HiddenNodes)
            {
                hiddenNode.Reset();
            }

            foreach (var outputNode in OutputNodes)
            {
                outputNode.Reset();
            }
        }

        private void ApplyVectorOnInputNodes(List<int> inputVector)
        {
            for (int i = 0; i < InputNodes.Count; i++)
            {
                InputNodes[i].Value = inputVector[i];
            }
        }

        private void AddWeightsToHiddenNodes()
        {
            foreach (var link in InputToHiddenSynapticLinks)
            {
                link.AddWeightToHiddenNode();
            }
        }

        private void ApplyActivationFunctionOnHiddenNodes()
        {
            foreach (var hiddenNode in HiddenNodes)
            {
                hiddenNode.ApplyActivationFunction();
            }
        }

        private void AddWeightsToOutputNodes()
        {
            foreach (var link in HiddenToOutputSynapticLinks)
            {
                link.AddWeightToOutputNode();
            }
        }

        private void ApplyActivationFunctionOnOutputNodes()
        {
            foreach (var outputNode in OutputNodes)
            {
                outputNode.ApplyActivationFunction();
            }
        }

        private string GetActivatedOutputNodeClass()
        {
            if (OutputNodes.Count(x => x.IsActivated) > 1)
            {
                throw new Exception("More than 1 output node was activated");
            }

            var activatedOutputNode = OutputNodes.FirstOrDefault(x => x.IsActivated);
            if (activatedOutputNode == null)
            {
                throw new Exception("No output node was activated");
            }

            return activatedOutputNode.Class;
        }

        private string GetTheMostActivatedOutputNodeClass()
        {
            if (!OutputNodes.Any(x => x.IsActivated))
            {
                throw new Exception("No output node was activated");
            }

            return OutputNodes.Where(x => x.IsActivated).OrderByDescending(x => Math.Abs(x.Value - x.Threshold) / x.Threshold).First().Class;
        }
    }
}
