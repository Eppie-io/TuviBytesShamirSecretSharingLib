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

using GF256Computations;
using System;
using System.Security.Cryptography;

namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Class realizes calculations used for Shamir's Secret Sharing algorithm. Based on SLIP-39 https://github.com/satoshilabs/slips/blob/master/slip-0039.md
    /// </summary>
    public static class SecretSharing
    {
        private const byte MaxAmountOfShares = 16;

        /// <summary>
        /// Interpolation function. Used to calculate f(x) using known points.
        /// </summary>
        /// <param name="x">x coordinate as value over GF(256).</param>
        /// <param name="points">Known points.</param>
        /// <returns>f(x).</returns>
        public static Field Interpolation(Field x, Point[] points)
        {
            Field result = new Field(0);
            for (int i = 0; i < points.Length; i++)
            {
                Field product = new Field(1);
                for (int j = 0; j < i; j++)
                {
                    product *= (x - points[j].X) / (points[i].X - points[j].X);
                }

                for (int j = i + 1; j < points.Length; j++)
                {
                    product *= (x - points[j].X) / (points[i].X - points[j].X);
                }

                result += points[i].Y * product;
            }

            return result;
        }

        /// <summary>
        /// Interpolation function. Used to calculate f(x) using known points.
        /// </summary>
        /// <param name="byteX">x coordinate as byte value.</param>
        /// <param name="bytePoints">Known points as tuple of bytes.</param>
        /// <returns>f(x).</returns>
        public static Field Interpolation(byte byteX, (byte, byte)[] bytePoints)
        {
            Field x = new Field(byteX);
            Point[] points = new Point[bytePoints.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point(bytePoints[i].Item1, bytePoints[i].Item2);
            }

            return Interpolation(x, points);
        }

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

            RandomNumberGenerator generator = RandomNumberGenerator.Create();
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
                result[i] = Interpolation(new Field(i), points).GetValue();
            }

            return result;
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
                throw new ArgumentException(nameof(shares));
            }

            if (shares.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 share to recover secret.", nameof(shares));
            }

            int size = shares[0].ShareValue.Length;
            foreach (var share in shares)
            {
                if (share.ShareValue.Length != size)
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
                    points[j] = new Point(shares[j].IndexNumber, shares[j].ShareValue[i]);
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
            return Interpolation(new Field(255), secretShares).GetValue();
        }

        /// <summary>
        /// Recovers main secret from shares. Secret is a byte.
        /// </summary>
        /// <param name="secretShares">Secret shares as tuple of bytes.</param>
        /// <returns>Main secret.</returns>
        public static byte RecoverSecret((byte, byte)[] secretShares)
        {
            Point[] points = new Point[secretShares.Length];
            for(byte i = 0; i < points.Length; i++)
            {
                points[i] = new Point(secretShares[i].Item1, secretShares[i].Item2);
            }

            return Interpolation(new Field(255), points).GetValue();
        }

        
    }
}
