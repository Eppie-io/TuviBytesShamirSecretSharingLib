///////////////////////////////////////////////////////////////////////////////
//   Copyright 2022 Eppie (https://eppie.io)
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

namespace GF256ComputationsTests
{
    public class FieldTests
    {        
        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(9, 4, 13)]
        [TestCase(45, 10, 39)]
        public void AdditionTests(byte a, byte b, byte result)
        {
            Field addendum1 = new Field(a);
            Field addendum2 = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = addendum1 + addendum2;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(2, 1, 3)]
        [TestCase(13, 4, 9)]
        [TestCase(45, 10, 39)]
        [TestCase(2, 253, 255)]
        public void SubtractionTests(byte a, byte b, byte result)
        {
            Field minuend = new Field(a);
            Field subtrahend = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = minuend - subtrahend;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(2, 1, 2)]
        [TestCase(222, 1, 222)]
        [TestCase(2, 2, 4)]
        [TestCase(2, 5, 10)]
        [TestCase(7, 3, 9)]
        [TestCase(7, 7, 21)]
        [TestCase(133, 3, 148)]
        [TestCase(202, 15, 74)]
        [TestCase(76, 94, 207)]
        public void MultiplyTests(byte a, byte b, byte result)
        {
            Field factor1 = new Field(a);
            Field factor2 = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = factor1 * factor2;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(155, 131, 181)]
        [TestCase(74, 15, 202)]
        [TestCase(207, 94, 76)]
        [TestCase(189, 111, 44)]
        [TestCase(223, 214, 178)]
        public void DivisionTests(byte a, byte b, byte result)
        {
            Field dividend = new Field(a);
            Field divider = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = dividend / divider;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestCase(208, 74)]
        [TestCase(17, 146)]
        [TestCase(196, 125)]
        [TestCase(134, 16)]
        public void MultiplyDivisionTests(byte a, byte b)
        {
            Field initialNumber = new Field(a);
            Field anyNumber = new Field(b);
            var actualResult = initialNumber * anyNumber / anyNumber;
            Assert.AreEqual(initialNumber, actualResult);
        }

        [TestCase(2, 3, 8)]
        [TestCase(3, 3, 15)]
        [TestCase(25, 14, 20)]
        [TestCase(139, 76, 174)]
        public void PowerTests(byte a, byte degree, byte result)
        {
            Field basis = new Field(a);
            Field expectedResult = new Field(result);
            var actualResult = Field.pow(basis, degree);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}