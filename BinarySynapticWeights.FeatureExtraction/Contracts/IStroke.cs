using System.Collections.Generic;
using System.Drawing;

namespace BinarySynapticWeights.FeatureExtraction.Contracts
{
    public interface IStroke
    {
        int GetNumberOfPoints();
        bool AppendPoint(Point point);
        List<Point> GetSignificantPoints();
        Point GetLastSignificantPoint();
    }
}
