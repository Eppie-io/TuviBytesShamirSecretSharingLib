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
using System.Collections.Generic;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class SecretSharingTests
    {
        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesInterpolation))]
        public void InterpolationTests(byte x, Point[] points, byte expectedResult)
        {
            SecretSharing s = new SecretSharing();
            var result = s.Interpolation(new Field(x), points);
            Assert.AreEqual(expectedResult, result.GetValue());
        }

        [TestCase (28)]
        [TestCase (144)]
        public void ThresholdOneTest(byte secret)
        {
            SecretSharing s = new();
            byte[] result = s.SplitSecret(1, 5, secret);
            foreach(var share in result)
            {
                Assert.AreEqual(secret, share);
            }
        }

        [TestCase(119)]
        [TestCase(231)]
        public void SecretRecoveryAllPossibilitiesTests(byte secret)
        {
            SecretSharing s = new();
            byte[] result = s.SplitSecret(3, 5, secret);
            Point point0 = new Point(0, result[0]);
            Point point1 = new Point(1, result[1]);
            Point point2 = new Point(2, result[2]);
            Point point3 = new Point(3, result[3]);
            Point point4 = new Point(4, result[4]);
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point1, point2 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point1, point3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point1, point4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point2, point3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point2, point4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point0, point3, point4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point1, point2, point3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point1, point2, point4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point1, point3, point4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Point[] { point2, point3, point4 }));
        }

        [TestCase(94)]
        [TestCase(167)]
        public void SecretRecoveryFailureNotEnoughSharesTests(byte secret)
        {
            SecretSharing s = new();
            byte[] result = s.SplitSecret(3, 5, secret);
            Point point0 = new Point(0, result[0]);
            Point point1 = new Point(1, result[1]);
            Point point2 = new Point(2, result[2]);
            Point point3 = new Point(3, result[3]);
            Point point4 = new Point(4, result[4]);
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point0, point1 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point0, point2 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point0, point3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point0, point4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point1, point2 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point1, point3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point1, point4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point2, point3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point2, point4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Point[] { point3, point4 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecoveryAllPossibilitiesTests1(byte[] secret)
        {
            SecretSharing s = new();
            var result = s.SplitSecret(3, 5, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share2 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share2, share3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share2, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share3, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share2, share3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share2, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share3, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share2, share3, share4 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecoveryAllPossibilitiesTests2(byte[] secret)
        {
            SecretSharing s = new();
            var result = s.SplitSecret(4, 6, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Share share5 = new Share(5, result[5].ShareValue);
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share2, share3 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share2, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share2, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share3, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share3, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share1, share4, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share2, share3, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share2, share3, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share2, share4, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share0, share3, share4, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share2, share3, share4 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share2, share3, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share2, share4, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share1, share3, share4, share5 }));
            Assert.AreEqual(secret, s.RecoverSecret(new Share[] { share2, share3, share4, share5 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecoveryFailureNotEnoughSharesTests(byte[] secret)
        {
            SecretSharing s = new();
            var result = s.SplitSecret(3, 5, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share0, share1 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share0, share2 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share0, share3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share0, share4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share1, share2 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share1, share3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share1, share4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share2, share3 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share2, share4 }));
            Assert.AreNotEqual(secret, s.RecoverSecret(new Share[] { share3, share4 }));
        }
    }
}