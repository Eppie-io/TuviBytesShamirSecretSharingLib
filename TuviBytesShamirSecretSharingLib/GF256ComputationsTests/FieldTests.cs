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
using TuviBytesShamirSecretSharingLibTests;

[assembly: CLSCompliant(true)]
namespace GF256ComputationsTests
{
    public class FieldTests
    {
        [Test]
        public void ConstructorTest()
        {
            Field field = new Field();
            Assert.That(field.Value, Is.Zero);
        }

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
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(1, 2, 3)]
        [TestCase(2, 1, 3)]
        [TestCase(9, 4, 13)]
        [TestCase(45, 10, 39)]
        public void AdditionAsFunctionTests(byte a, byte b, byte result)
        {
            Field addendum1 = new Field(a);
            Field addendum2 = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = Field.Add(addendum1, addendum2);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(2, 1, 3)]
        [TestCase(13, 4, 9)]
        [TestCase(45, 10, 39)]
        [TestCase(2, 253, 255)]
        public void SubtractionAsFunctionTests(byte a, byte b, byte result)
        {
            Field minuend = new Field(a);
            Field subtrahend = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = Field.Subtract(minuend, subtrahend);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
        public void MultiplicationTests(byte a, byte b, byte result)
        {
            Field factor1 = new Field(a);
            Field factor2 = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = factor1 * factor2;
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
        public void MultiplicationAsFunctionTests(byte a, byte b, byte result)
        {
            Field factor1 = new Field(a);
            Field factor2 = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = Field.Multiply(factor1, factor2);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(155, 131, 181)]
        [TestCase(74, 15, 202)]
        [TestCase(207, 94, 76)]
        [TestCase(189, 111, 44)]
        [TestCase(223, 214, 178)]
        public void DivisionAsFunctionTests(byte a, byte b, byte result)
        {
            Field dividend = new Field(a);
            Field divider = new Field(b);
            Field expectedResult = new Field(result);
            var actualResult = Field.Divide(dividend, divider);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
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
            Assert.That(actualResult, Is.EqualTo(initialNumber));
        }

        [TestCase(2, 3, 8)]
        [TestCase(3, 3, 15)]
        [TestCase(25, 14, 20)]
        [TestCase(139, 76, 174)]
        public void PowerTests(byte a, byte degree, byte result)
        {
            Field basis = new Field(a);
            Field expectedResult = new Field(result);
            var actualResult = Field.Pow(basis, degree);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCase(2, 3, false)]
        [TestCase(3, 3, true)]
        [TestCase(25, 14, false)]
        [TestCase(139, 139, true)]
        [TestCase(null, 251, false)]
        [TestCase(182, null, false)]
        public void EqualsTests(byte a, byte b, bool expectedResult)
        {
            Field left = new Field(a);
            Field right = new Field(b);
            var actualResult = left.Equals(right);
            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForEquality))]
        public void EqualsOperatorTests(Field left, Field right, bool expectedResult)
        {
            Assert.That(left == right, Is.EqualTo(expectedResult));
            Assert.That(left != right, Is.Not.EqualTo(expectedResult));
        }

        [TestCase(134)]
        [TestCase(22)]
        [TestCase(253)]
        [TestCase(178)]
        public void GetHashCodeTests(int value)
        {
            Field field = new Field((byte)value);
            int result = field.GetHashCode();
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void ToStringTests()
        {
            Field field = new Field(15);
            var result = field.ToString();
            Assert.That(result, Is.EqualTo("15"));

            field = new Field(234);
            result = field.ToString();
            Assert.That(result, Is.EqualTo("234"));
        }

        [TestCase(189, 0)]
        [TestCase(223, 0)]
        public void DivisionByZeroThrowArgumentException(byte a, byte b)
        {
            Field dividend = new Field(a);
            Field divider = new Field(b);
            Assert.Throws<ArgumentException>(() => Field.Divide(dividend, divider),
                message: "Share array can not be a null.");
        }
    }
}