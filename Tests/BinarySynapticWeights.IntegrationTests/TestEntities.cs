using System.Collections.Generic;

namespace BinarySynapticWeights.IntegrationTests
{
    public static class TestEntities
    {
        public static List<Sample> GetTrainingSamples()
        {
            return new List<Sample>
            {
                GetSample1(),
                GetSample2(),
                GetSample3(),
                GetSample4(),
                GetSample5()
            };
        }

        public static Sample GetSample1()
        {
            return new Sample
            {
                InputVector = new List<int> { 0, 0, 0 },
                OutputClass = "black"
            };
        }

        public static Sample GetSample2()
        {
            return new Sample
            {
                InputVector = new List<int> { 1, 1, 0 },
                OutputClass = "black"
            };
        }

        public static Sample GetSample3()
        {
            return new Sample
            {
                InputVector = new List<int> { 0, 0, 1 },
                OutputClass = "white"
            };
        }

        public static Sample GetSample4()
        {
            return new Sample
            {
                InputVector = new List<int> { 1, 0, 0 },
                OutputClass = "white"
            };
        }

        public static Sample GetSample5()
        {
            return new Sample
            {
                InputVector = new List<int> { 1, 1, 1 },
                OutputClass = "white"
            };
        }
    }
}
