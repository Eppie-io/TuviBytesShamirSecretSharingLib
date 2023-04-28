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

using System;

namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Secret share. It contains partitional secret of Shamir's secret sharing scheme.
    /// </summary>
    public class Share
    {
        private const byte MaxIndexNumber = 15;

        /// <summary>
        /// Share's index number. Value of point X in scheme for this share.
        /// </summary>
        public byte IndexNumber { get; }

        private readonly byte[] shareValue;

        /// <summary>
        /// Share's value. Value f(X) of secret share for each byte in array.
        /// </summary>
        public byte[] GetShareValue()
        {
            return shareValue;
        }

        /// <summary>
        /// Constructor of secret share.
        /// </summary>
        /// <param name="index">Index number of share.</param>
        /// <param name="value">Share's value.</param>
        public Share(byte index, byte[] value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (index > MaxIndexNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Share's index number can not be bigger than {MaxIndexNumber}.");
            }

            IndexNumber = index;
            shareValue = (new byte[value.Length]);
            for (int i = 0; i < value.Length; i++)
            {
                GetShareValue()[i] = value[i];
            }
        }
    }
}
