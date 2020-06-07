using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class BSWNeuralNetwork : IBSWNeuralNetwork
    {
        private readonly Model model;

        public BSWNeuralNetwork(Model model)
        {
            this.model = model;
        }

        public string PredictClass(List<int> inputVector)
        {
            if (model.OutputNodes.Count == 0)
            {
                throw new InvalidOperationException("Binary Synaptic Weights neural network is not trained");
            }

            if (inputVector.Count != model.InputNodes.Count)
            {
                throw new ArgumentException($"{nameof(inputVector)} should have the same size as the samples used to train the network: {model.InputNodes.Count}");
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
            foreach (var hiddenNode in model.HiddenNodes)
            {
                hiddenNode.Reset();
            }

            foreach (var outputNode in model.OutputNodes)
            {
                outputNode.Reset();
            }
        }

        private void ApplyVectorOnInputNodes(List<int> inputVector)
        {
            for (int i = 0; i < model.InputNodes.Count; i++)
            {
                model.InputNodes[i].Value = inputVector[i];
            }
        }

        private void AddWeightsToHiddenNodes()
        {
            foreach (var link in model.InputToHiddenSynapticLinks)
            {
                link.AddWeightToHiddenNode();
            }
        }

        private void ApplyActivationFunctionOnHiddenNodes()
        {
            foreach (var hiddenNode in model.HiddenNodes)
            {
                hiddenNode.ApplyActivationFunction();
            }
        }

        private void AddWeightsToOutputNodes()
        {
            foreach (var link in model.HiddenToOutputSynapticLinks)
            {
                link.AddWeightToOutputNode();
            }
        }

        private void ApplyActivationFunctionOnOutputNodes()
        {
            foreach (var outputNode in model.OutputNodes)
            {
                outputNode.ApplyActivationFunction();
            }
        }

        private string GetActivatedOutputNodeClass()
        {
            if (model.OutputNodes.Count(x => x.IsActivated) > 1)
            {
                throw new Exception("More than 1 output node was activated");
            }

            var activatedOutputNode = model.OutputNodes.FirstOrDefault(x => x.IsActivated);
            if (activatedOutputNode == null)
            {
                throw new Exception("No output node was activated");
            }

            return activatedOutputNode.Class;
        }

        private string GetTheMostActivatedOutputNodeClass()
        {
            if (!model.OutputNodes.Any(x => x.IsActivated))
            {
                throw new Exception("No output node was activated");
            }

            return model.OutputNodes.Where(x => x.IsActivated).OrderByDescending(x => Math.Abs(x.Value - x.Threshold) / x.Threshold).First().Class;
        }
    }
}
