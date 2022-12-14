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

        private byte value;

        public Field()
        {
            value = 0;
        }

        public Field(byte value)
        {
            this.value = value;
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

        /// <summary>
        /// Getter.
        /// </summary>
        /// <returns></returns>
        public byte GetValue()
        {
            return value;
        }

        /// <summary>
        /// Setter.
        /// </summary>
        /// <param name="_value"></param>
        public void SetValue(byte value)
        {
            this.value = value;
        }

        //operators
        public static explicit operator Field(byte b)
        {
            Field f = new Field(b);
            return f;
        }

        public static explicit operator byte(Field f)
        {
            return f.value;
        }

        public static Field operator +(Field Fa, Field Fb)
        {
            byte bres = (byte)(Fa.value ^ Fb.value);
            return new Field(bres);
        }

        public static Field operator -(Field Fa, Field Fb)
        {
            byte bres = (byte)(Fa.value ^ Fb.value);
            return new Field(bres);
        }

        public static Field operator *(Field Fa, Field Fb)
        {
            Field FRes = new Field(0);
            if (Fa.value != 0 && Fb.value != 0)
            {
                byte bres = (byte)((Log[Fa.value] + Log[Fb.value]) % (order - 1));
                bres = Exp[bres];
                FRes.value = bres;
            }
            return FRes;
        }

        public static Field operator /(Field Fa, Field Fb)
        {
            if (Fb.value == 0)
            {
                throw new System.ArgumentException("Divisor cannot be 0", "Fb");
            }

            Field Fres = new Field(0);
            if (Fa.value != 0)
            {
                byte bres = (byte)(((order - 1) + Log[Fa.value] - Log[Fb.value]) % (order - 1));
                bres = Exp[bres];
                Fres.value = bres;
            }
            return Fres;
        }

        public static Field pow(Field f, byte exp)
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
            return (Fa.value == Fb.value);
        }

        public static bool operator !=(Field Fa, Field Fb)
        {
            return !(Fa.value == Fb.value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Field F = obj as Field;
            if ((System.Object)F == null)
            {
                return false;
            }
            return (value == F.value);
        }

#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override int GetHashCode()
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString();
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
    }
}
