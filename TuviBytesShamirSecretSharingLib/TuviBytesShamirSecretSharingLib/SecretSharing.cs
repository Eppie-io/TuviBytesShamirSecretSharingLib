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
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("TuviBytesShamirSecretSharingLibTests")]
namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Class realizes calculations used for Shamir's Secret Sharing algorithm. Based on SLIP-39 https://github.com/satoshilabs/slips/blob/master/slip-0039.md
    /// </summary>
    public static class SecretSharing
    {
        private const byte MaxAmountOfShares = 16;
        private const int MaxAmountOfPoints = byte.MaxValue + 1;
                
        /// <summary>
        /// Simple version of secret splitting. Secret is an array of bytes.
        /// </summary>
        /// <param name="threshold">Threshold. Minimal amount of shares to recover secret.</param>
        /// <param name="numberOfShares">Amount of shares.</param>
        /// <param name="secret">Secret.</param>
        /// <returns>Array of shares.</returns>
        public static Share[] SplitSecret(byte threshold, byte numberOfShares, byte[] secret)
        {
            return SplitSecret(threshold, numberOfShares, secret, RandomNumberGenerator.Create);
        }

        internal static Share[] SplitSecret(byte threshold, byte numberOfShares, byte[] secret, Func<RandomNumberGenerator> createGenerator)
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
                byte[] subResult = SplitSecret(threshold, numberOfShares, secret[i], createGenerator);
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
            return SplitSecret(threshold, numberOfShares, secret, RandomNumberGenerator.Create);
        }

        internal static byte[] SplitSecret(byte threshold, byte numberOfShares, byte secret, Func<RandomNumberGenerator> createGenerator)
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

            using (RandomNumberGenerator generator = createGenerator())
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
        /// <param name="shares">Between 1 and 16 shares with distinct indices and equal lengths.</param>
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

            if (shares.Length > MaxAmountOfShares)
            {
                throw new ArgumentException($"Too many shares, max amount - {MaxAmountOfShares}.", nameof(shares));
            }

            int size = shares[0].GetShareValue().Length;
            bool[] usedIndices = new bool[MaxAmountOfShares];
            foreach (var share in shares)
            {
                if (share.GetShareValue().Length != size)
                {
                    throw new ArgumentException("Your shares have different size.");
                }

                if (usedIndices[share.IndexNumber])
                {
                    throw new ArgumentException("Shares must have distinct indices.", nameof(shares));
                }

                usedIndices[share.IndexNumber] = true;
            }

            byte[] resultSecret = new byte[size];

            for (int i = 0; i < size; i++)
            {
                Point[] points = new Point[shares.Length];
                for (int j = 0; j < shares.Length; j++)
                {
                    points[j] = new Point(shares[j].IndexNumber, shares[j].GetShareValue()[i]);
                }

                resultSecret[i] = Interpolation.Interpolate(new Field(255), points).Value;
            }

            return resultSecret;
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is a byte.
        /// </summary>
        /// <param name="secretShares">Between 1 and 256 points with distinct X coordinates.</param>
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

            if (secretShares.Length > MaxAmountOfPoints)
            {
                throw new ArgumentException($"Too many points, max amount - {MaxAmountOfPoints}.", nameof(secretShares));
            }

            bool[] usedCoordinates = new bool[MaxAmountOfPoints];
            foreach (var share in secretShares)
            {
                if (usedCoordinates[share.X.Value])
                {
                    throw new ArgumentException("Points must have distinct X coordinates.", nameof(secretShares));
                }

                usedCoordinates[share.X.Value] = true;
            }

            return Interpolation.Interpolate(new Field(255), secretShares).Value;
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is a byte.
        /// </summary>
        /// <param name="secretShares">Between 1 and 256 byte tuples with distinct X coordinates.</param>
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

            if (secretShares.Length > MaxAmountOfPoints)
            {
                throw new ArgumentException($"Too many points, max amount - {MaxAmountOfPoints}.", nameof(secretShares));
            }

            Point[] points = new Point[secretShares.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point(secretShares[i].Item1, secretShares[i].Item2);
            }

            return RecoverSecret(points);
        }
    }
}
