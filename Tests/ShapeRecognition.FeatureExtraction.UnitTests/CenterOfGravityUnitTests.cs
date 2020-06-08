using System.Collections.Generic;
using System.Drawing;
using ShapeRecognition.FeatureExtraction.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ShapeRecognition.FeatureExtraction.UnitTests
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
        public void TestThatWhenShapeIsSquareGetCenterOfGravityReturnsSquareCenter()
        {
            var stroke1 = new Mock<IStroke>();
            var stroke2 = new Mock<IStroke>();
            var stroke3 = new Mock<IStroke>();
            var stroke4 = new Mock<IStroke>();
            stroke1.Setup(x => x.GetSignificantPoints()).Returns(new List<Point> { new Point(0, 0), new Point(4, 0) });
            stroke2.Setup(x => x.GetSignificantPoints()).Returns(new List<Point> { new Point(4, 0), new Point(4, 1), new Point(4, 2), new Point(4, 4) });
            stroke3.Setup(x => x.GetSignificantPoints()).Returns(new List<Point> { new Point(4, 4), new Point(0, 4) });
            stroke4.Setup(x => x.GetSignificantPoints()).Returns(new List<Point> { new Point(0, 4), new Point(0, 0) });
            var strokes = new List<IStroke> { stroke1.Object, stroke2.Object, stroke3.Object, stroke4.Object };
            var expectedX = 2;
            var expectedY = 2;

            var result = centerOfGravity.GetCenterOfGravity(strokes);

            Assert.AreEqual(expectedX, result.X);
            Assert.AreEqual(expectedY, result.Y);
        }
    }
}
