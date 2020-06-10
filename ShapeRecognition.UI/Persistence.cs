using BinarySynapticWeights.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShapeRecognition
{
    public class Persistence
    {
        public void SaveTrainingSamplesToFile(List<Sample> trainingSamples, string filePath)
        {
            var serializedSamples = JsonConvert.SerializeObject(trainingSamples);

            File.WriteAllText(filePath, serializedSamples);
        }

        public void SaveTrainingSamplesToFile(List<Sample> trainingSamples)
        {
            var serializedSamples = JsonConvert.SerializeObject(trainingSamples);

            var outputFileName = $"SavedTrainingSamples\\TrainingSamples.{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.json";

            File.WriteAllText(outputFileName, serializedSamples);
        }

        public List<Sample> ReadTrainingSamplesFromFile(string filePath)
        {
            var serializedSamples = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<List<Sample>>(serializedSamples);
        }

        public void SaveModelToFile(Model bswModel)
        {
            var serializedModel = JsonConvert.SerializeObject(bswModel);

            var outputFileName = $"SavedModels\\SavedModel.{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.json";

            File.WriteAllText(outputFileName, serializedModel);
        }

        public Model ReadModelFromFile(string filePath)
        {
            var serializedModel = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<Model>(serializedModel);
        }
    }
}
