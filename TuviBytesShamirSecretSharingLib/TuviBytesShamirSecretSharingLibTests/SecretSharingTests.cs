using NUnit.Framework;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class SecretSharingTests
    {
        //[TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesInterpolation))]
        //public void InterpolationTests(byte x, (byte, byte)[] points, byte expectedResult)
        //{
        //    SecretSharing s = new SecretSharing();
        //    var result = s.Interpolation(x, points);
        //    Assert.AreEqual(expectedResult, result.GetValue());
        //}

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
        public void SecretRecoveryNotEnoughSharesTests(byte secret)
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
    }
}