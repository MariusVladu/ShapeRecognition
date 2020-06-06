using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySynapticWeights
{
    public class Vectors
    {
        private readonly static HammingDistance HammingDistance = new HammingDistance();

        public List<int> GetAverageVector(string outputClass, List<Sample> samples)
        {
            var samplesOfThisClass = samples.Where(x => x.OutputClass == outputClass);
            var numberOfSamples = samplesOfThisClass.Count();

            var vectorLength = samplesOfThisClass.FirstOrDefault().InputVector.Count;
            var averageVector = new List<int>();

            for (int i = 0; i < vectorLength; i++)
            {
                double sum = 0;
                foreach (var sample in samplesOfThisClass)
                {
                    sum += sample.InputVector[i];
                }

                averageVector.Add((int)Math.Round(sum / numberOfSamples));
            }

            return averageVector;
        }

        public Sample GetKeySample(List<int> averageVector, string outputClass, List<Sample> samples)
        {
            var samplesOfThisClass = samples.Where(x => x.OutputClass == outputClass);

            var closestSample = samplesOfThisClass.FirstOrDefault();
            var minimumHammingDistance = int.MaxValue;

            foreach (var sample in samplesOfThisClass)
            {
                var distance = HammingDistance.GetHammingDistance(averageVector, sample.InputVector);

                if (distance < minimumHammingDistance)
                {
                    closestSample = sample;
                    minimumHammingDistance = distance;
                }
            }

            return closestSample;
        }

        public Sample GetYesSample(Sample key, List<Sample> samples)
        {
            var samplesOfThisClass = samples.Where(x => x.OutputClass == key.OutputClass);

            var furthestSample = samplesOfThisClass.FirstOrDefault();
            var maximumHammingDistance = int.MinValue;

            foreach (var sample in samplesOfThisClass)
            {
                var distance = HammingDistance.GetHammingDistance(key, sample);

                if (distance > maximumHammingDistance)
                {
                    furthestSample = sample;
                    maximumHammingDistance = distance;
                }
            }

            return furthestSample;
        }

        public Sample GetNoSample(Sample key, List<Sample> samples)
        {
            var samplesOfOtherClasses = samples.Where(x => x.OutputClass != key.OutputClass);

            var closestSample = samplesOfOtherClasses.FirstOrDefault();
            var minimumHammingDistance = int.MaxValue;

            foreach (var sample in samplesOfOtherClasses)
            {
                var distance = HammingDistance.GetHammingDistance(key, sample);

                if (distance < minimumHammingDistance)
                {
                    closestSample = sample;
                    minimumHammingDistance = distance;
                }
            }

            return closestSample;
        }
    }
}
