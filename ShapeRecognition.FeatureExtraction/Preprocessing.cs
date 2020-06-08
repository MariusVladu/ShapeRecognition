using BinarySynapticWeights;
using BinarySynapticWeights.Entities;
using ShapeRecognition.FeatureExtraction.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ShapeRecognition.FeatureExtraction
{
    public class Preprocessing
    {
        private int serialCodeLength;

        public Preprocessing(int serialCodeLength)
        {
            this.serialCodeLength = serialCodeLength;
        }

        public Sample GetTrainingSampleFromShapeFeatures(ShapeFeatures shapeFeatures, ShapeType shapeType)
        {
            return new Sample
            {
                InputVector = GetInputVectorFromShapeFeatures(shapeFeatures),
                OutputClass = shapeType.ToString()
            };
        }

        public List<int> GetInputVectorFromShapeFeatures(ShapeFeatures shapeFeatures)
        {
            var acuteSerialCode = SerialCoding.GetSeriallyCodedValue(shapeFeatures.AcuteAngles, serialCodeLength);
            var rightSerialCode = SerialCoding.GetSeriallyCodedValue(shapeFeatures.RightAngles, serialCodeLength);
            var wideSerialCode = SerialCoding.GetSeriallyCodedValue(shapeFeatures.WideAngles, serialCodeLength);
            var straightSerialCode = SerialCoding.GetSeriallyCodedValue(shapeFeatures.AcuteAngles, serialCodeLength);

            return acuteSerialCode.Concat(rightSerialCode).Concat(wideSerialCode).Concat(straightSerialCode).ToList();
        }
    }
}
