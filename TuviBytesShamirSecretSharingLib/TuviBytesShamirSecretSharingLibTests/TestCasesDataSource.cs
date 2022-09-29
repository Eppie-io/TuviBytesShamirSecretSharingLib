using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuviBytesShamirSecretSharingLibTests
{
    public static class TestCasesDataSource
    {
        public static IEnumerable<TestCaseData> TestCasesForBytesInterpolation
        {
            get
            {
                yield return new TestCaseData( //y = x - 1
                    8, 
                    new (byte, byte)[] { (1, 0), (2, 3) }, 
                    9);
                yield return new TestCaseData( //y = x - 1
                    13,
                    new (byte, byte)[] { (1, 0), (2, 3) },
                    12);
                yield return new TestCaseData( //y = x^2 + 3x + 5
                    3,
                    new (byte, byte)[] { (0, 5), (1, 7), (2, 3) },
                    15);
            }
        }        
    }
}
