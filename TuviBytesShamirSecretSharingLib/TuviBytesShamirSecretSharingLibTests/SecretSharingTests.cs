using NUnit.Framework;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class SecretSharingTests
    {
        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForBytesInterpolation))]
        public void InterpolationTests(byte x, (byte, byte)[] points, byte expectedResult)
        {
            SecretSharing s = new SecretSharing();
            var result = s.Interpolation(x, points);
            Assert.AreEqual(expectedResult, result.GetValue());
        }
    }
}