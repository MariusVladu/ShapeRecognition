namespace BinarySynapticWeights
{
    public class HiddenToOutputSynapticLink
    {
        public HiddenNode HiddenNode { get; set; }
        public OutputNode OutputNode { get; set; }

        public void AddWeightToOutputNode()
        {
            if(HiddenNode.IsActivated)
            {
                OutputNode.Value += HiddenNode.Value;
            }
        }
    }
}
