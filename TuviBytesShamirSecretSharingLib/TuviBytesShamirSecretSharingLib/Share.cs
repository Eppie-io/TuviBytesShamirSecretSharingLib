using System;
using System.Collections.Generic;
using System.Text;

namespace TuviBytesShamirSecretSharingLib
{
    public class Share
    {
        public byte IndexNumber { get; }

        public byte[] ShareValue { get; }

        public Share(byte index, byte[] value)
        {
            IndexNumber = index;
            ShareValue = new byte[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                ShareValue[i] = value[i];
            }
        }
    }
}
