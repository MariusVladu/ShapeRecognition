using BinarySynapticWeights.Nodes;
using BinarySynapticWeights.SynapticLinks;
using System.Collections.Generic;

namespace BinarySynapticWeights.Entities
{
    public class Model
    {
        public List<InputNode> InputNodes { get; set; }
        public List<HiddenNode> HiddenNodes { get; set; }
        public List<OutputNode> OutputNodes { get; set; }

        public List<InputToHiddenSynapticLink> InputToHiddenSynapticLinks { get; set; }
        public List<HiddenToOutputSynapticLink> HiddenToOutputSynapticLinks { get; set; }
    }
}
