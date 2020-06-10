using BinarySynapticWeights.Contracts;
using BinarySynapticWeights.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class BSWNeuralNetwork : IBSWNeuralNetwork
    {
        public Model Model { get; set; }

        public BSWNeuralNetwork(Model model)
        {   
            this.Model = model;
        }

        public string PredictClass(List<int> inputVector)
        {
            if (Model.OutputNodes.Count == 0)
            {
                throw new InvalidOperationException("Binary Synaptic Weights neural network is not trained");
            }

            if (inputVector.Count != Model.InputNodes.Count)
            {
                throw new ArgumentException($"{nameof(inputVector)} should have the same size as the samples used to train the network: {Model.InputNodes.Count}");
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
            foreach (var hiddenNode in Model.HiddenNodes)
            {
                hiddenNode.Reset();
            }

            foreach (var outputNode in Model.OutputNodes)
            {
                outputNode.Reset();
            }
        }

        private void ApplyVectorOnInputNodes(List<int> inputVector)
        {
            for (int i = 0; i < Model.InputNodes.Count; i++)
            {
                Model.InputNodes[i].Value = inputVector[i];
            }
        }

        private void AddWeightsToHiddenNodes()
        {
            foreach (var link in Model.InputToHiddenSynapticLinks)
            {
                link.AddWeightToHiddenNode();
            }
        }

        private void ApplyActivationFunctionOnHiddenNodes()
        {
            foreach (var hiddenNode in Model.HiddenNodes)
            {
                hiddenNode.ApplyActivationFunction();
            }
        }

        private void AddWeightsToOutputNodes()
        {
            foreach (var link in Model.HiddenToOutputSynapticLinks)
            {
                link.AddWeightToOutputNode();
            }
        }

        private void ApplyActivationFunctionOnOutputNodes()
        {
            foreach (var outputNode in Model.OutputNodes)
            {
                outputNode.ApplyActivationFunction();
            }
        }

        private string GetActivatedOutputNodeClass()
        {
            if (Model.OutputNodes.Count(x => x.IsActivated) > 1)
            {
                throw new Exception("More than 1 output node was activated");
            }

            var activatedOutputNode = Model.OutputNodes.FirstOrDefault(x => x.IsActivated);
            if (activatedOutputNode == null)
            {
                throw new Exception("No output node was activated");
            }

            return activatedOutputNode.Class;
        }

        private string GetTheMostActivatedOutputNodeClass()
        {
            if (!Model.OutputNodes.Any(x => x.IsActivated))
            {
                throw new Exception("No output node was activated");
            }

            return Model.OutputNodes.Where(x => x.IsActivated).OrderByDescending(x => Math.Abs(x.Value - x.Threshold) / x.Threshold).First().Class;
        }
    }
}
