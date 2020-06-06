using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BinarySynapticWeights.UnitTests
{
    [TestClass]
    public class SerialCodingUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestThatWhenValueIsGreaterThanLengthGetSeriallyCodedValueThrowsArgumentException()
        {
            SerialCoding.GetSeriallyCodedValue(6, 5);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestThatWhenValueIsNegativeGetSeriallyCodedValueThrowsArgumentException()
        {
            var result = SerialCoding.GetSeriallyCodedValue(-1, 5);
        }

        [TestMethod]
        public void TestThatGetSeriallyCodedValueReturnsExpectedSerialCodingFor0()
        {
            var length = 5;
            var value = 0;
            var expectedSerialCoding = new List<int> { 0, 0, 0, 0, 0 };

            var result = SerialCoding.GetSeriallyCodedValue(value, length);

            CollectionAssert.AreEqual(expectedSerialCoding, result);
        }

        [TestMethod]
        public void TestThatGetSeriallyCodedValueReturnsExpectedSerialCodingFor3()
        {
            var length = 5;
            var value = 3;
            var expectedSerialCoding = new List<int> { 0, 0, 1, 1, 1 };

            var result = SerialCoding.GetSeriallyCodedValue(value, length);

            CollectionAssert.AreEqual(expectedSerialCoding, result);
        }

        [TestMethod]
        public void TestThatGetSeriallyCodedValueReturnsExpectedSerialCodingFor5()
        {
            var length = 5;
            var value = 5;
            var expectedSerialCoding = new List<int> { 1, 1, 1, 1, 1 };

            var result = SerialCoding.GetSeriallyCodedValue(value, length);

            CollectionAssert.AreEqual(expectedSerialCoding, result);
        }
    }
}
