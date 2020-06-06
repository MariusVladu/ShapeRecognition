using BinarySynapticWeights.Nodes;

namespace BinarySynapticWeights.SynapticLinks
{
    public class InputToHiddenSynapticLink
    {
        public InputNode InputNode { get; set; }
        public HiddenNode HiddenNode { get; set; }
        public int Weight { get; set; }

        public void AddWeightToHiddenNode()
        {
            HiddenNode.Value += InputNode.Value * Weight;
        }
    }
}
