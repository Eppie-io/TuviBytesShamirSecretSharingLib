using GF256Computations;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuviBytesShamirSecretSharingLib
{
    /// <summary>
    /// Represents a point of graphic over GF(256).
    /// </summary>
    public class Point
    {
        public Field X { get; set; }
        public Field Y { get; set; }

        public Point (byte x, byte y)
        {
            X = new Field(x);
            Y = new Field(y);
        }
    }
}
