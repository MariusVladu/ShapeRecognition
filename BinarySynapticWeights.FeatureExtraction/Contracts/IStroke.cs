using System.Collections.Generic;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction.Contracts
{
    public interface IStroke
    {
        int GetNumberOfPoints();
        void AppendPoint(Point point);
        List<Point> GetSignificantPoints();
    }
}
