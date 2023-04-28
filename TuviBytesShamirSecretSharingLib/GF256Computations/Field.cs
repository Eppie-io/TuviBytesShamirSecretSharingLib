///////////////////////////////////////////////////////////////////////////////
//   Based on project https://github.com/fauzanhilmi/GaloisField
//   This project is licensed under the MIT License
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;

namespace GF256Computations
{
    /// <summary>
    /// This class contains all computations in GF(256). Field's elements are represented as polynomials.
    /// </summary>
    public class Field
    {
        public const int order = 256;
        //irreducible polynomial used : x^8 + x^4 + x^3 + x + 1 (0x11B)
        public const int polynomial = 0x11B;
        //generator to be used in Exp & Log table generation (a primitive element of this finite field: x + 1 (0x3))
        public const byte generator = 0x3;
        protected readonly static byte[] Exp = new byte[order];
        protected readonly static byte[] Log = new byte[order];

        public Field()
        {
            Value = 0;
        }

        public Field(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Generates Exp & Log tables for fast multiplication/division calculations.
        /// </summary>
        static Field()
        {
            byte val = 0x01;
            for (int i = 0; i < order; i++)
            {
                Exp[i] = val;
                if (i < order - 1)
                {
                    Log[val] = (byte)i;
                }
                val = Multiply(generator, val);
            }
        }

        public byte Value { get; set; }

        //operators
        public static explicit operator Field(byte b)
        {
            Field f = new Field(b);
            return f;
        }

        public static explicit operator byte(Field f)
        {
            if (f is null)
            {
                throw new ArgumentNullException(nameof(f));
            }

            return f.Value;
        }

        public static Field operator +(Field Fa, Field Fb)
        {
            if (Fa is null)
            {
                throw new ArgumentNullException(nameof(Fa));
            }
            if (Fb is null)
            {
                throw new ArgumentNullException(nameof(Fb));
            }

            byte bres = (byte)(Fa.Value ^ Fb.Value);
            return new Field(bres);
        }

        public static Field operator -(Field Fa, Field Fb)
        {
            if (Fa is null)
            {
                throw new ArgumentNullException(nameof(Fa));
            }
            if (Fb is null)
            {
                throw new ArgumentNullException(nameof(Fb));
            }

            byte bres = (byte)(Fa.Value ^ Fb.Value);
            return new Field(bres); 
        }

        public static Field operator *(Field Fa, Field Fb)
        {
            if (Fa is null)
            {
                throw new ArgumentNullException(nameof(Fa));
            }
            if (Fb is null)
            {
                throw new ArgumentNullException(nameof(Fb));
            }

            Field FRes = new Field(0);
            if (Fa.Value != 0 && Fb.Value != 0)
            {
                byte bres = (byte)((Log[Fa.Value] + Log[Fb.Value]) % (order - 1));
                bres = Exp[bres];
                FRes.Value = bres;
            }
            return FRes;
        }

        public static Field operator /(Field Fa, Field Fb)
        {
            if (Fa is null)
            {
                throw new ArgumentNullException(nameof(Fa));
            }
            if (Fb is null)
            {
                throw new ArgumentNullException(nameof(Fb));
            }

            if (Fb.Value == 0)
            {
                throw new ArgumentException("Divisor cannot be 0", nameof(Fb));
            }

            Field Fres = new Field(0);
            if (Fa.Value != 0)
            {
                byte bres = (byte)(((order - 1) + Log[Fa.Value] - Log[Fb.Value]) % (order - 1));
                bres = Exp[bres];
                Fres.Value = bres;
            }
            return Fres;
        }

        public static Field Pow(Field f, byte exp)
        {
            Field fres = new Field(1);
            for (byte i = 0; i < exp; i++)
            {
                fres *= f;
            }
            return fres;
        }

        public static bool operator ==(Field Fa, Field Fb)
        {
            if (Fa is null || Fb is null)
            {
                return false;
            }

            return Fa.Value == Fb.Value;
        }

        public static bool operator !=(Field Fa, Field Fb)
        {
            if (Fa is null || Fb is null)
            {
                return true;
            }

            return !(Fa.Value == Fb.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Field F = obj as Field;
            if ((Object)F == null)
            {
                return false;
            }
            return (Value == F.Value);
        }

#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override int GetHashCode()
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString(new CultureInfo("en-us"));
        }

        /// <summary>
        /// Multiplication method which is only used in Exp & Log table generation.
        /// Implemented with Russian Peasant Multiplication algorithm.
        /// </summary>
        /// <param name="a">Factor1.</param>
        /// <param name="b">Factor2.</param>
        /// <returns>Result of multiplication.</returns>
        private static byte Multiply(byte a, byte b)
        {
            byte result = 0;
            byte aa = a;
            byte bb = b;
            while (bb != 0)
            {
                if ((bb & 1) != 0)
                {
                    result ^= aa;
                }
                byte highest_bit = (byte)(aa & 0x80);
                aa <<= 1;
                if (highest_bit != 0)
                {
                    aa ^= (polynomial & 0xFF);
                }
                bb >>= 1;
            }
            return result;
        }

        public static Field Multiply(Field left, Field right)
        {
            return left * right;
        }

        public static Field Divide(Field left, Field right)
        {
            return left / right;
        }

        public static Field Add(Field left, Field right)
        {
            return left + right;
        }

        public static Field Subtract(Field left, Field right)
        {
            return left - right;
        }

        public static Field ToField(byte byteToField)
        {
            return (Field)byteToField;
        }

        public static byte ToByte(Field fieldToByte)
        {
            return (byte)fieldToByte;
        }
    }
}