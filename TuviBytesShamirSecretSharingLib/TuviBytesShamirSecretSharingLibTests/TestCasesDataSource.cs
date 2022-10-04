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

using NUnit.Framework;
using System.Collections.Generic;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    internal static class TestCasesDataSource
    {
        public static IEnumerable<TestCaseData> TestCasesForBytesInterpolation
        {
            get
            {
                yield return new TestCaseData(
                    (byte)8, new Point[] { new Point(1, 0), new Point(2, 3) }, (byte)9); //y = x - 1
                yield return new TestCaseData(
                    (byte)13, new Point[] { new Point(1, 0), new Point(2, 3) }, (byte)12); //y = x - 1
                yield return new TestCaseData(
                    (byte)3, new Point[] { new Point(0, 5), new Point(1, 7), new Point(2, 7) }, (byte)5); //y = x^2 + 3x + 5
            }
        }

        public static IEnumerable<byte[]> TestCasesForBytesArraySecretRecovery
        {
            get
            {
                yield return new byte[] { 253, 19, 175, 128, 64, 201 };
                yield return new byte[] { 33, 44, 55, 69, 115, 221, 186, 2, 201, 191, 117 };
                yield return new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            }
        }
    }
}
