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

namespace TuviBytesShamirSecretSharingLib
{
    public static class Interpolation
    {
        /// <summary>
        /// Interpolation function. Used to calculate f(x) using known points.
        /// </summary>
        /// <param name="x">x coordinate as value over GF(256).</param>
        /// <param name="points">Known points.</param>
        /// <returns>f(x).</returns>
        public static Field Interpolate(Field x, Point[] points)
        {
            if (points is null)
            {
                throw new ArgumentNullException(nameof(points));
            }

            if (points.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 point to interpolate function.", nameof(points));
            }

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
        public static Field Interpolate(byte byteX, (byte, byte)[] bytePoints)
        {
            if (bytePoints is null)
            {
                throw new ArgumentNullException(nameof(bytePoints));
            }

            if (bytePoints.Length < 1)
            {
                throw new ArgumentException("You should send at least 1 point to interpolate function.", nameof(bytePoints));
            }

            Field x = new Field(byteX);
            Point[] points = new Point[bytePoints.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Point(bytePoints[i].Item1, bytePoints[i].Item2);
            }

            return Interpolate(x, points);
        }

    }
}
