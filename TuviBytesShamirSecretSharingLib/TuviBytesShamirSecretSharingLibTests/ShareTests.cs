using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TuviBytesShamirSecretSharingLib;

namespace TuviBytesShamirSecretSharingLibTests
{
    public class ShareTests
    {
        [TestCaseSource(typeof(TestCasesDataSource), nameof(TestCasesDataSource.TestCasesForShareCreation))]
        public void ShareCreation(byte index, byte[] value)
        {
            Share share = new Share(index, value);
            Assert.AreEqual(index, share.IndexNumber);
            Assert.AreEqual(value, share.GetShareValue());
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
            Assert.Throws<ArgumentNullException>(() => new Share (1, null),
                message: "Share value can not be a null.");
        }
    }
}
