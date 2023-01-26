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
    public class SecretSharingTests
    {
        [TestCase (28)]
        [TestCase (144)]
        public void ThresholdOneTest(byte secret)
        {
            byte[] result = SecretSharing.SplitSecret(1, 5, secret);
            foreach(var share in result)
            {
                Assert.AreEqual(secret, share);
            }
        }

        [TestCase(119)]
        [TestCase(231)]
        public void SecretRecovery_AllPossibilitiesTests(byte secret)
        {
            byte[] result = SecretSharing.SplitSecret(3, 5, secret);
            Point point0 = new Point(0, result[0]);
            Point point1 = new Point(1, result[1]);
            Point point2 = new Point(2, result[2]);
            Point point3 = new Point(3, result[3]);
            Point point4 = new Point(4, result[4]);
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point1, point2 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point1, point3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point1, point4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point2, point3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point2, point4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point3, point4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point2, point3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point2, point4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point3, point4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Point[] { point2, point3, point4 }));
        }

        [TestCase(94)]
        [TestCase(167)]
        public void SecretRecovery_FailureNotEnoughSharesTests(byte secret)
        {
            byte[] result = SecretSharing.SplitSecret(3, 5, secret);
            Point point0 = new Point(0, result[0]);
            Point point1 = new Point(1, result[1]);
            Point point2 = new Point(2, result[2]);
            Point point3 = new Point(3, result[3]);
            Point point4 = new Point(4, result[4]);
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point1 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point2 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point0, point4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point2 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point1, point4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point2, point3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point2, point4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Point[] { point3, point4 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecovery_AllPossibilitiesTests1(byte[] secret)
        {
            var result = SecretSharing.SplitSecret(3, 5, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share2 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2, share3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share3, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2, share3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share3, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share2, share3, share4 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecovery_AllPossibilitiesTests2(byte[] secret)
        {
            Share[] result = SecretSharing.SplitSecret(4, 6, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Share share5 = new Share(5, result[5].ShareValue);
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share3 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share3, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share3, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1, share4, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2, share3, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2, share3, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2, share4, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share3, share4, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2, share3, share4 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2, share3, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2, share4, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share3, share4, share5 }));
            Assert.AreEqual(secret, SecretSharing.RecoverSecret(new Share[] { share2, share3, share4, share5 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecovery_FailureNotEnoughSharesTests(byte[] secret)
        {
            Share[] result = SecretSharing.SplitSecret(3, 5, secret);
            Share share0 = new Share(0, result[0].ShareValue);
            Share share1 = new Share(1, result[1].ShareValue);
            Share share2 = new Share(2, result[2].ShareValue);
            Share share3 = new Share(3, result[3].ShareValue);
            Share share4 = new Share(4, result[4].ShareValue);
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share1 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share2 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share0, share4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share2 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share1, share4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share2, share3 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share2, share4 }));
            Assert.AreNotEqual(secret, SecretSharing.RecoverSecret(new Share[] { share3, share4 }));
        }

        [Test]
        public void RecoverSecret_ShareArrayIsNull_ThrowArgumentNullException()
        {
            Share[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecret_PointArrayIsNull_ThrowArgumentNullException()
        {
            Point[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecret_ByteTupleArrayIsNull_ThrowArgumentNullException()
        {
            (byte, byte)[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecret_ShareArrayIsEmpty_ThrowArgumentException()
        {
            Share[] shares = new Share[0];
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void RecoverSecret_PointArrayIsEmpty_ThrowArgumentException()
        {
            Point[] shares = new Point[0];
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void RecoverSecret_ByteTupleArrayIsEmpty_ThrowArgumentException()
        {
            (byte, byte)[] shares = new (byte, byte)[0];
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void SplitSecret_SecretIsNull_ThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SecretSharing.SplitSecret(3, 5, null),
                message: "Secret can not be a null.");
        }

        [Test]
        public void SplitSecret_SecretIsEmptyArray_ThrowArgumentException()
        {
            byte[] array = new byte[0];
            Assert.Throws<ArgumentException>(() => SecretSharing.SplitSecret(3, 5, array),
                message: "Secret array should have at least one byte to split secret.");
        }

        [Test]
        public void SplitSecret_ThresholdIsZero_ThrowArgumentOutOfRangeException()
        {
            byte[] secret = new byte[5] { 45, 76, 192, 219, 14};
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(0, 5, secret),
                message: "Threshold can not be 0.");
        }

        [Test]
        public void SplitSecret_ThresholdIsBiggerThanShares_ThrowArgumentException()
        {
            byte[] secret = new byte[5] { 45, 76, 192, 219, 14 };
            Assert.Throws<ArgumentException>(() => SecretSharing.SplitSecret(6, 5, secret),
                message: "Threshold can not be bigger than number of shares.");
        }

        [Test]
        public void SplitSecret_NumberOfSharesIsTooBig_ThrowAArgumentOutOfRangeException()
        {
            byte[] array = new byte[] { 15, 93 };
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(5, 17, array),
                message: "Too many shares, max amount - 16.");
        }
    }
}