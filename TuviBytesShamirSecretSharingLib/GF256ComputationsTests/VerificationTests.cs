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

namespace GF256ComputationsTests
{
    public class VerificationTests
    {
        [Test]
        public void TransformationToByteTest()
        {
            Field field = new Field(197);
            byte b = Field.ToByte(field);
            Assert.That(b, Is.EqualTo((byte)field));
        }

        [Test]
        public void TransformationToByteThrowArgumentNullException()
        {
            Field f = null;
            Assert.Throws<ArgumentNullException>(() => Field.ToByte(f),
                message: "Share array can not be a null.");
        }

        [Test]
        public void TransformationToFieldTest()
        {
            Field f = Field.ToField(5);
            Assert.That(f, Is.EqualTo((Field)5));
        }

        [Test]
        public void AddNullArgumentsThrowArgumentNullException()
        {
            Field left = null;
            Field right = new Field(11);
            Assert.Throws<ArgumentNullException>(() => Field.Add(left, right),
                message: "Share array can not be a null.");
            left = new Field(15);
            right = null;
            Assert.Throws<ArgumentNullException>(() => Field.Add(left, right),
                message: "Share array can not be a null.");
        }

        [Test]
        public void SubtractNullArgumentsThrowArgumentNullException()
        {
            Field left = null;
            Field right = new Field(11);
            Assert.Throws<ArgumentNullException>(() => Field.Subtract(left, right),
                message: "Share array can not be a null.");
            left = new Field(15);
            right = null;
            Assert.Throws<ArgumentNullException>(() => Field.Subtract(left, right),
                message: "Share array can not be a null.");
        }


        [Test]
        public void MultiplyNullArgumentsThrowArgumentNullException()
        {
            Field left = null;
            Field right = new Field(11);
            Assert.Throws<ArgumentNullException>(() => Field.Multiply(left, right),
                message: "Share array can not be a null.");
            left = new Field(15);
            right = null;
            Assert.Throws<ArgumentNullException>(() => Field.Multiply(left, right),
                message: "Share array can not be a null.");
        }

        [Test]
        public void DivideNullArgumentsThrowArgumentNullException()
        {
            Field left = null;
            Field right = new Field(11);
            Assert.Throws<ArgumentNullException>(() => Field.Divide(left, right),
                message: "Share array can not be a null.");
            left = new Field(15);
            right = null;
            Assert.Throws<ArgumentNullException>(() => Field.Divide(left, right),
                message: "Share array can not be a null.");
        }
    }
}
