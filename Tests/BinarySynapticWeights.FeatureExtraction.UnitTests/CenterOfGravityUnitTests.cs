using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinarySynapticWeights.FeatureExtraction.UnitTests
{
    [TestClass]
    public class CenterOfGravityUnitTests
    {
        private CenterOfGravity centerOfGravity;

        [TestInitialize]
        public void Setup()
        {
            centerOfGravity = new CenterOfGravity();
        }

        [TestMethod]
        public void TestThatGetSegmentLengthReturnsExpectedLength()
        {
            var p1 = new Point(1, 3);
            var p2 = new Point(3, 3);
            var expectedResult = 2;

            var result = centerOfGravity.GetSegmentLength(p1, p2);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestThatGetSegmentXAverageReturnsExpectedLength()
        {
            var p1 = new Point(1, 3);
            var p2 = new Point(3, 3);
            var expectedResult = 2;

            var result = centerOfGravity.GetSegmentXAverage(p1, p2);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestThatGetSegmentYAverageReturnsExpectedLength()
        {
            var p1 = new Point(1, 1);
            var p2 = new Point(1, 4);
            var expectedResult = 2.5;

            var result = centerOfGravity.GetSegmentYAverage(p1, p2);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestThatGetTotalShapeLengthReturnsExpectedLength()
        {
            var shapePoints = new List<Point> { new Point(0, 0), new Point(2, 0), new Point(2, 2), new Point(0, 2) };
            var expectedTotalLength = 6;

            var result = centerOfGravity.GetTotalShapeLength(shapePoints);

            Assert.AreEqual(expectedTotalLength, result);
        }

        [TestMethod]
        public void TestThatWhenShapeIsSquareGetCenterOfGravityReturnsSquareCenter()
        {
            var shapePoints = new List<Point> { new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4) };
            var expectedX = 2;
            var expectedY = 2;

            var result = centerOfGravity.GetCenterOfGravity(shapePoints);

            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }
    }
}
