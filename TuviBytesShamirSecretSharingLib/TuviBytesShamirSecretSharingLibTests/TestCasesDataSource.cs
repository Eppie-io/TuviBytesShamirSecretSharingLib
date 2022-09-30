using NUnit.Framework;
using System.Collections.Generic;

namespace TuviBytesShamirSecretSharingLibTests
{
    internal static class TestCasesDataSource
    {
        public static IEnumerable<TestCaseData> TestCasesForBytesInterpolation
        {
            get
            {
                yield return new TestCaseData(
                    (byte)8, new (byte, byte)[] { (1, 0), (2, 3) }, (byte)9); //y = x - 1
                yield return new TestCaseData(
                    (byte)13, new (byte, byte)[] { (1, 0), (2, 3) }, (byte)12); //y = x - 1
                yield return new TestCaseData(
                    (byte)3, new (byte, byte)[] { (0, 5), (1, 7), (2, 7) }, (byte)5); //y = x^2 + 3x + 5
            }
        }        
    }
}
