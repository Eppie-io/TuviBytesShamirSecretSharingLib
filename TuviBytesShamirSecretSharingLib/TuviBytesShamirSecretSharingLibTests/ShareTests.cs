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

using NUnit.Framework;
using System;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class ShareTests
    {
        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForShareCreation))]
        public void ShareCreation(byte index, byte[] value)
        {
            Share share = new Share(index, value);
            Assert.That(share.IndexNumber, Is.EqualTo(index));
            Assert.That(share.GetShareValue(), Is.EqualTo(value));
        }

        [TestCase(16)]
        [TestCase(129)]
        public void ShareCreationTooBigIndexThrowArgumentOutOfRangeException(byte index)
        {
            byte[] values = new byte[] { 11, 98, 214, 184 };
            Assert.Throws<ArgumentOutOfRangeException>(() => new Share(index, values),
                message: "Share's index number can not be bigger than 15.");
        }

        [Test]
        public void ShareCreationValueIsNullThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Share(1, null),
                message: "Share value can not be a null.");
        }
    }
}
