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
using System.Collections.Generic;
using System.Security.Cryptography;
using TuviBytesShamirSecretSharingLib;

[assembly: CLSCompliant(true)]
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
                Assert.That(share, Is.EqualTo(secret));
            }
        }

        [TestCase(119)]
        [TestCase(231)]
        public void SecretRecoveryAsByteTuplesAllPossibilitiesTests(byte secret)
        {
            byte[] result = SecretSharing.SplitSecret(2, 3, secret);

            var recoverSecret1 = SecretSharing.RecoverSecret(new (byte, byte)[] { (0, result[0]), (1, result[1]) });
            var recoverSecret2 = SecretSharing.RecoverSecret(new (byte, byte)[] { (0, result[0]), (2, result[2]) });
            var recoverSecret3  = SecretSharing.RecoverSecret(new (byte, byte)[] { (1, result[1]), (2, result[2]) });

            Assert.That(recoverSecret1, Is.EqualTo(secret));
            Assert.That(recoverSecret2, Is.EqualTo(secret));
            Assert.That(recoverSecret3, Is.EqualTo(secret));
        }

        [TestCase(119)]
        [TestCase(231)]
        public void SecretRecoveryAllPossibilitiesTests(byte secret)
        {
            byte[] result = SecretSharing.SplitSecret(3, 5, secret);
            Point point0 = new Point(0, result[0]);
            Point point1 = new Point(1, result[1]);
            Point point2 = new Point(2, result[2]);
            Point point3 = new Point(3, result[3]);
            Point point4 = new Point(4, result[4]);

            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1, point2 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1, point3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1, point4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point2, point3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point2, point4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point3, point4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point1, point2, point3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point1, point2, point4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point1, point3, point4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point2, point3, point4 }), Is.EqualTo(secret));
        }

        [Test]
        public void SecretRecoverySameTwoSharesCanBelongToDifferentSecrets()
        {
            Point point0 = new Point(0, 7);
            Point point1 = new Point(1, 4);
            Point linearPoint2 = new Point(2, 1);
            Point quadraticPoint2 = new Point(2, 7);

            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1, linearPoint2 }), Is.EqualTo(29));
            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1, quadraticPoint2 }), Is.EqualTo(241));

            Assert.That(SecretSharing.RecoverSecret(new Point[] { point0, point1 }), Is.EqualTo(29));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecoveryAllPossibilitiesTests1(byte[] secret)
        {
            var result = SecretSharing.SplitSecret(3, 5, secret);
            Share share0 = new Share(0, result[0].GetShareValue());
            Share share1 = new Share(1, result[1].GetShareValue());
            Share share2 = new Share(2, result[2].GetShareValue());
            Share share3 = new Share(3, result[3].GetShareValue());
            Share share4 = new Share(4, result[4].GetShareValue());
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share2 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share2, share3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share2, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share3, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share2, share3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share2, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share3, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share2, share3, share4 }), Is.EqualTo(secret));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesArraySecretRecovery))]
        public void ArraySecretRecoveryAllPossibilitiesTests2(byte[] secret)
        {
            Share[] result = SecretSharing.SplitSecret(4, 6, secret);
            Share share0 = new Share(0, result[0].GetShareValue());
            Share share1 = new Share(1, result[1].GetShareValue());
            Share share2 = new Share(2, result[2].GetShareValue());
            Share share3 = new Share(3, result[3].GetShareValue());
            Share share4 = new Share(4, result[4].GetShareValue());
            Share share5 = new Share(5, result[5].GetShareValue());
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share3 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share2, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share3, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share3, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, share4, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share2, share3, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share2, share3, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share2, share4, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share3, share4, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share2, share3, share4 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share2, share3, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share2, share4, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share1, share3, share4, share5 }), Is.EqualTo(secret));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share2, share3, share4, share5 }), Is.EqualTo(secret));
        }

        [Test]
        public void SingleByteArraySecretRecoverySameTwoSharesCanBelongToDifferentSecrets()
        {
            Share share0 = new Share(0, new byte[] { 7 });
            Share share1 = new Share(1, new byte[] { 4 });
            Share linearShare2 = new Share(2, new byte[] { 1 });
            Share quadraticShare2 = new Share(2, new byte[] { 7 });

            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, linearShare2 }), Is.EqualTo(new byte[] { 29 }));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, quadraticShare2 }), Is.EqualTo(new byte[] { 241 }));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1 }), Is.EqualTo(new byte[] { 29 }));
        }

        [Test]
        public void ArraySecretRecoverySameTwoSharesCanBelongToDifferentSecrets()
        {
            Share share0 = new Share(0, new byte[] { 7, 0, 255 });
            Share share1 = new Share(1, new byte[] { 4, 1, 0 });
            Share linearShare2 = new Share(2, new byte[] { 1, 2, 26 });
            Share quadraticShare2 = new Share(2, new byte[] { 7, 4, 28 });

            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, linearShare2 }), Is.EqualTo(new byte[] { 29, 255, 236 }));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1, quadraticShare2 }), Is.EqualTo(new byte[] { 241, 19, 0 }));
            Assert.That(SecretSharing.RecoverSecret(new Share[] { share0, share1 }), Is.EqualTo(new byte[] { 29, 255, 236 }));
        }

        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForSecretSplitting))]
        public void SplitSecretWithKnownRandomBytesProducesExpectedShares(byte threshold, byte secret, byte[] randomBytes, byte[] expected)
        {
            byte[] shares = SecretSharing.SplitSecret(threshold, 5, secret,
                () => new FixedRandomNumberGenerator(randomBytes));

            Assert.That(shares, Is.EqualTo(expected));
        }

        [Test]
        public void ArraySplitSecretWithKnownRandomBytesProducesExpectedShares()
        {
            byte[] secret = new byte[] { 241, 19, 0 };
            var randomBlocks = new Queue<byte[]>(new byte[][]
            {
                new byte[] { 7, 4 },
                new byte[] { 0, 1 },
                new byte[] { 255, 0 }
            });
            byte[][] expected = new byte[][]
            {
                new byte[] { 7, 0, 255 },
                new byte[] { 4, 1, 0 },
                new byte[] { 7, 4, 28 },
                new byte[] { 4, 5, 227 },
                new byte[] { 31, 16, 58 }
            };

            Share[] shares = SecretSharing.SplitSecret(3, 5, secret,
                () => new FixedRandomNumberGenerator(randomBlocks.Dequeue()));

            Assert.That(shares, Has.Length.EqualTo(expected.Length));
            for (int i = 0; i < shares.Length; i++)
            {
                Assert.That(shares[i].IndexNumber, Is.EqualTo(i));
                Assert.That(shares[i].GetShareValue(), Is.EqualTo(expected[i]));
            }
            Assert.That(randomBlocks, Is.Empty);
        }

        [TestCase(255)]
        [TestCase(256)]
        [TestCase(257)]
        public void ArraySecretRecoveryAcrossByteIndexBoundary(int length)
        {
            byte[] secret = new byte[length];
            for (int i = 0; i < secret.Length; i++)
            {
                secret[i] = (byte)((i * 73 + 19 + i / 256) % 256);
            }

            Share[] shares = SecretSharing.SplitSecret(3, 5, secret);
            byte[] recovered = SecretSharing.RecoverSecret(new Share[] { shares[4], shares[1], shares[3] });

            Assert.That(recovered, Is.EqualTo(secret));
        }

        [TestCase(1)]
        [TestCase(16)]
        public void ArraySecretRecoverySupportsShareCountBoundaries(byte count)
        {
            byte[] secret = new byte[] { 17, 71, 255 };
            Share[] shares = SecretSharing.SplitSecret(count, count, secret);

            Assert.That(SecretSharing.RecoverSecret(shares), Is.EqualTo(secret));
        }

        [TestCase(17)]
        [TestCase(256)]
        [TestCase(257)]
        public void ArraySecretRecoveryRejectsTooManyShares(int count)
        {
            Share[] shares = new Share[count];
            for (int i = 0; i < shares.Length; i++)
            {
                shares[i] = new Share((byte)(i % 16), new byte[] { 42 });
            }

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("shares"));
        }

        [Test]
        public void ArraySecretRecoveryRejectsDuplicateIndices()
        {
            Share[] shares = new Share[] { new Share(0, new byte[] { 7 }), new Share(0, new byte[] { 4 }) };

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("shares"));
        }

        [TestCase(255)]
        [TestCase(256)]
        public void SecretRecoverySupportsByteTupleCountBoundaries(int count)
        {
            var shares = new (byte, byte)[count];
            for (int i = 0; i < shares.Length; i++)
            {
                shares[i] = ((byte)i, 42);
            }

            Assert.That(SecretSharing.RecoverSecret(shares), Is.EqualTo(42));
        }

        [TestCase(255)]
        [TestCase(256)]
        public void SecretRecoverySupportsPointCountBoundaries(int count)
        {
            Point[] shares = new Point[count];
            for (int i = 0; i < shares.Length; i++)
            {
                shares[i] = new Point((byte)i, 42);
            }

            Assert.That(SecretSharing.RecoverSecret(shares), Is.EqualTo(42));
        }

        [Test]
        public void SecretRecoveryRejectsTooManyByteTuples()
        {
            var shares = new (byte, byte)[257];
            for (int i = 0; i < shares.Length; i++)
            {
                shares[i] = ((byte)i, 42);
            }

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("secretShares"));
        }

        [Test]
        public void SecretRecoveryRejectsTooManyPoints()
        {
            Point[] shares = new Point[257];
            for (int i = 0; i < shares.Length; i++)
            {
                shares[i] = new Point((byte)i, 42);
            }

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("secretShares"));
        }

        [Test]
        public void SecretRecoveryRejectsDuplicateByteTupleCoordinates()
        {
            var shares = new (byte, byte)[] { (0, 7), (0, 4) };

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("secretShares"));
        }

        [Test]
        public void SecretRecoveryRejectsDuplicatePointCoordinates()
        {
            Point[] shares = new Point[] { new Point(0, 7), new Point(0, 4) };

            ArgumentException exception = Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares));
            Assert.That(exception.ParamName, Is.EqualTo("secretShares"));
        }

        [Test]
        public void RecoverSecretSharesHaveDifferentSizeThrowArgumentNullException()
        {
            Share[] shares = new Share[] {
                new Share(1, new byte[] {1, 2, 3}),
                new Share(2, new byte[] {1, 2, 3, 4}),
                new Share(5, new byte[] {11, 12, 13, 14, 15})};
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "Your shares have different size.");
        }

        [Test]
        public void RecoverSecretShareArrayIsNullThrowArgumentNullException()
        {
            Share[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecretPointArrayIsNullThrowArgumentNullException()
        {
            Point[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecretByteTupleArrayIsNullThrowArgumentNullException()
        {
            (byte, byte)[] shares = null;
            Assert.Throws<ArgumentNullException>(() => SecretSharing.RecoverSecret(shares),
                message: "Share array can not be a null.");
        }

        [Test]
        public void RecoverSecretShareArrayIsEmptyThrowArgumentException()
        {
            Share[] shares = Array.Empty<Share>();
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void RecoverSecretPointArrayIsEmptyThrowArgumentException()
        {
            Point[] shares = Array.Empty<Point>();
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void RecoverSecretByteTupleArrayIsEmptyThrowArgumentException()
        {
            (byte, byte)[] shares = Array.Empty<(byte, byte)>();
            Assert.Throws<ArgumentException>(() => SecretSharing.RecoverSecret(shares),
                message: "You should send at least 1 share to recover secret.");
        }

        [Test]
        public void SplitSecretSecretIsNullThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => SecretSharing.SplitSecret(3, 5, null),
                message: "Secret can not be a null.");
        }

        [Test]
        public void SplitSecretSecretIsEmptyArrayThrowArgumentException()
        {
            byte[] secret = Array.Empty<byte>();
            Assert.Throws<ArgumentException>(() => SecretSharing.SplitSecret(3, 5, secret),
                message: "Secret array should have at least one byte to split secret.");
        }

        [Test]
        public void SplitSecretThresholdIsZeroThrowArgumentOutOfRangeException()
        {
            byte[] secret = new byte[5] { 45, 76, 192, 219, 14};
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(0, 5, secret),
                message: "Threshold can not be 0.");
        }

        [Test]
        public void SplitSecretThresholdIsBiggerThanSharesThrowArgumentException()
        {
            byte[] secret = new byte[5] { 45, 76, 192, 219, 14 };
            Assert.Throws<ArgumentException>(() => SecretSharing.SplitSecret(6, 5, secret),
                message: "Threshold can not be bigger than number of shares.");
        }

        [Test]
        public void SplitSecretNumberOfSharesIsTooBigThrowAArgumentOutOfRangeException()
        {
            byte[] secret = new byte[] { 15, 93 };
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(5, 17, secret),
                message: "Too many shares, max amount - 16.");
        }

        [Test]
        public void SplitSecretByteThresholdIsZeroThrowArgumentOutOfRangeException()
        {
            byte secret = 18;
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(0, 5, secret),
                message: "Threshold can not be 0.");
        }

        [Test]
        public void SplitSecretByteThresholdIsBiggerThanSharesThrowArgumentException()
        {
            byte secret = 18;
            Assert.Throws<ArgumentException>(() => SecretSharing.SplitSecret(6, 5, secret),
                message: "Threshold can not be bigger than number of shares.");
        }

        [Test]
        public void SplitSecretByteNumberOfSharesIsTooBigThrowAArgumentOutOfRangeException()
        {
            byte secret = 18;
            Assert.Throws<ArgumentOutOfRangeException>(() => SecretSharing.SplitSecret(5, 17, secret),
                message: "Too many shares, max amount - 16.");
        }

        private sealed class FixedRandomNumberGenerator : RandomNumberGenerator
        {
            private readonly byte[] bytes;

            public FixedRandomNumberGenerator(byte[] bytes)
            {
                this.bytes = bytes;
            }

            public override void GetBytes(byte[] data)
            {
                if (data is null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                Assert.That(data, Has.Length.EqualTo(bytes.Length));
                Array.Copy(bytes, data, data.Length);
            }
        }
    }
}