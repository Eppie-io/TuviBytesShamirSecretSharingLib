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

using GF256Computations;
using System;
using System.Security.Cryptography;

[assembly: CLSCompliant(true)]
namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Class realizes calculations used for Shamir's Secret Sharing algorithm. Based on SLIP-39 https://github.com/satoshilabs/slips/blob/master/slip-0039.md
    /// </summary>
    public static class SecretSharing
    {
        private const byte MaxAmountOfShares = 16;
                
        /// <summary>
        /// Simple version of secret splitting. Secret is an array of bytes.
        /// </summary>
        /// <param name="threshold">Threshold. Minimal amount of shares to recover secret.</param>
        /// <param name="numberOfShares">Amount of shares.</param>
        /// <param name="secret">Secret.</param>
        /// <returns>Array of shares.</returns>
        public static Share[] SplitSecret(byte threshold, byte numberOfShares, byte[] secret)
        {
            if (secret is null)
            {
                throw new ArgumentNullException(nameof(secret));
            }

            if (secret.Length < 1)
            {
                throw new ArgumentException("Secret array should have at least one byte to split secret.", nameof(secret));
            }

            if (threshold == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold can not be 0.");
            }

            if (threshold > numberOfShares)
            {
                throw new ArgumentException("Threshold can not be bigger than number of shares.");
            }

            if (numberOfShares > MaxAmountOfShares)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfShares), $"Too many shares, max amount - {MaxAmountOfShares}.");
            }

            byte[][] result = new byte[numberOfShares][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new byte[secret.Length];
            }

            for (int i = 0; i < secret.Length; i++)
            {
                byte[] subResult = SplitSecret(threshold, numberOfShares, secret[i]);
                for (int j = 0; j < subResult.Length; j++)
                {
                    result[j][i] = subResult[j];
                }
            }

            Share[] shares = new Share[numberOfShares];
            for (byte i = 0; i < result.Length; i++)
            {
                shares[i] = new Share(i, result[i]);
            }

            return shares;
        }

        /// <summary>
        /// Simple version of secret splitting. Secret is byte.
        /// </summary>
        /// <param name="threshold">Threshold. Minimal amount of shares to recover secret.</param>
        /// <param name="numberOfShares">Amount of shares.</param>
        /// <param name="secret">Secret.</param>
        /// <returns>Array of shares.</returns>
        public static byte[] SplitSecret(byte threshold, byte numberOfShares, byte secret)
        {
            if (threshold == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold can not be 0.");
            }

            if (threshold > numberOfShares)
            {
                throw new ArgumentException("Threshold can not be bigger than number of shares.");
            }

            if (numberOfShares > MaxAmountOfShares)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfShares), $"Too many shares, max amount - {MaxAmountOfShares}.");
            }

            byte[] result = new byte[numberOfShares];
            Point[] points = new Point[threshold];

            if (threshold == 1)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = secret;
                }

                return result;
            }

            using (RandomNumberGenerator generator = RandomNumberGenerator.Create())
            {
                byte[] random = new byte[threshold - 1];
                generator.GetBytes(random);

                for (byte i = 0; i < threshold - 1; i++)
                {
                    result[i] = random[i];
                    points[i] = new Point(i, random[i]);
                }

                points[threshold - 1] = new Point(255, secret);

                for (byte i = (byte)(threshold - 1); i < numberOfShares; i++)
                {
                    result[i] = Interpolation.Interpolate(new Field(i), points).Value;
                }

                return result;
            }
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is an array of bytes.
        /// </summary>
        /// <param name="shares"></param>
        /// <returns>Recovered secret.</returns>
        public static byte[] RecoverSecret(Share[] shares)
        {
            if (shares is null)
            {
                throw new ArgumentNullException(nameof(shares));
            }

            if (shares.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 share to recover secret.", nameof(shares));
            }

            int size = shares[0].GetShareValue().Length;
            foreach (var share in shares)
            {
                if (share.GetShareValue().Length != size)
                {
                    throw new ArgumentException("Your shares have different size.");
                }
            }

            byte[] resultSecret = new byte[size];

            for (byte i = 0; i < size; i++)
            {
                Point[] points = new Point[shares.Length];
                for (byte j = 0; j < shares.Length; j++)
                {
                    points[j] = new Point(shares[j].IndexNumber, shares[j].GetShareValue()[i]);
                }

                resultSecret[i] = RecoverSecret(points);
            }

            return resultSecret;
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is a byte.
        /// </summary>
        /// <param name="secretShares">Secret shares as points.</param>
        /// <returns>Main secret.</returns>
        public static byte RecoverSecret(Point[] secretShares)
        {
            if (secretShares is null)
            {
                throw new ArgumentNullException(nameof(secretShares));
            }

            if (secretShares.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 share to recover secret.", nameof(secretShares));
            }

            return Interpolation.Interpolate(new Field(255), secretShares).Value;
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is a byte.
        /// </summary>
        /// <param name="secretShares">Secret shares as tuple of bytes.</param>
        /// <returns>Main secret.</returns>
        public static byte RecoverSecret((byte, byte)[] secretShares)
        {
            if (secretShares is null)
            {
                throw new ArgumentNullException(nameof(secretShares));
            }

            if (secretShares.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 share to recover secret.", nameof(secretShares));
            }

            Point[] points = new Point[secretShares.Length];
            for(byte i = 0; i < points.Length; i++)
            {
                points[i] = new Point(secretShares[i].Item1, secretShares[i].Item2);
            }

            return Interpolation.Interpolate(new Field(255), points).Value;
        }

        
    }
}
