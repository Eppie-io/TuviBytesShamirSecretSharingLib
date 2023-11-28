///////////////////////////////////////////////////////////////////////////////
//   Copyright 2023 Eppie (https://eppie.io)
//
//   Licensed under the Apache License, Version 2.0(the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
///////////////////////////////////////////////////////////////////////////////

using GF256Computations;
using NUnit.Framework;
using System;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class InterpolationTests
    {
        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForPointsInterpolation))]
        public void InterpolateFromPointsCorrectWorkTests(byte x, Point[] points, byte expectedResult)
        {
            var result = Interpolation.Interpolate(new Field(x), points);
            Assert.That(result.Value, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesInterpolation))]
        public void InterpolateFromByteTuplesCorrectWorkTests(byte x, byte[] xValues, byte[] yValues, byte expectedResult)
        {
            if (xValues is null)
            {
                throw new ArgumentNullException(nameof(xValues));
            }
            if (yValues is null)
            {
                throw new ArgumentNullException(nameof(yValues));
            }

            (byte, byte)[] points = new (byte, byte)[xValues.Length];
            for (int i = 0; i < xValues.Length; i++)
            {
                points[i] = (xValues[i], yValues[i]);
            }

            var result = Interpolation.Interpolate(x, points);
            Assert.That(result.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public void InterpolateFromPointsPointsIsNullThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Interpolation.Interpolate(new Field(3), null),
                message: "Points can not be a null.");
        }

        [Test]
        public void InterpolateFromPointsPointsIsIsEmptyArrayThrowArgumentException()
        {
            Point[] points = Array.Empty<Point>();
            Assert.Throws<ArgumentException>(() => Interpolation.Interpolate(new Field(3), points),
                message: "There are should be at least 1 point to interpolate function.");
        }
        
        [Test]
        public void InterpolateFromByteTuplesPointsIsNullThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Interpolation.Interpolate(3, null),
                message: "Points can not be a null.");
        }

        [Test]
        public void InterpolateFromByteTuplesPointsIsIsEmptyArrayThrowArgumentException()
        {
            (byte,byte)[] points = Array.Empty<(byte, byte)>();
            Assert.Throws<ArgumentException>(() => Interpolation.Interpolate(3, points),
                message: "There are should be least 1 point to interpolate function.");
        }
    }
}
