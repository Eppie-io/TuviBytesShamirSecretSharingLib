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
