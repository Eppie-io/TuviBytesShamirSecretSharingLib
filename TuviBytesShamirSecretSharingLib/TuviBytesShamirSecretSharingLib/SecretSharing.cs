using GF256Computations;
using System;

namespace TuviBytesShamirSecretSharingLib
{
    public class SecretSharing
    {
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
    }
}
