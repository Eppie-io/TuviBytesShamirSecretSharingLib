using GF256Computations;
using System;
using System.Security.Cryptography;

namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Class realizes calculations used for Shamir's Secret Sharing algorithm. Based on SLIP-39 https://github.com/satoshilabs/slips/blob/master/slip-0039.md
    /// </summary>
    public class SecretSharing
    {
        private const byte MaxAmountOfSHares = 16;

        public Field Interpolation(Field x, Point[] points)
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

        public Field Interpolation(byte byteX, (byte, byte)[] bytePoints)
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
        /// Simple version of secret splitting
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="numberOfShares"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public byte[] SplitSecret(byte threshold, byte numberOfShares, byte secret)
        {
            if (threshold == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold can not be 0.");
            }

            if (threshold > numberOfShares)
            {
                throw new ArgumentException("Threshold can not be bigger than number of shares.");
            }

            if (numberOfShares > MaxAmountOfSHares)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfShares), $"Too many shares, max amount - {MaxAmountOfSHares}.");
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

            // we skip steps with calculating and using digest D

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

        public byte RecoverSecret(Point[] secretShares)
        {
            return Interpolation(new Field(255), secretShares).GetValue();
        }

        public byte RecoverSecret((byte, byte)[] secretShares)
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
