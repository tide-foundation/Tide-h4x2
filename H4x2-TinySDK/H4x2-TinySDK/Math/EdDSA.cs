// 
// Tide Protocol - Infrastructure for a TRUE Zero-Trust paradigm
// Copyright (C) 2022 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Code License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Code License for more details.
// You should have received a copy of the Tide Community Open 
// Code License along with this program.
// If not, see https://tide.org/licenses_tcoc2-0-0-en
//

using H4x2_TinySDK.Ed25519;
using System.Numerics;
using H4x2_TinySDK.Tools;

namespace H4x2_TinySDK.Math;
    public class EdDSA
    {

        /// <summary>
        /// EdDSA signing with point Ed25519
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <returns>A Point R and BigInteger s.</returns>
        public static (Point R, BigInteger S) Ed25519Sign(byte[] message, Key key)
        {
            BigInteger r, s;
            Point R;
            do
            {   
                // Random r instead of hash(hash(privateKey) + msg) mod q
                r = Utils.RandomBigInt(Curve.N);
                R = (Curve.G * r);
            } while (R.GetX() % Curve.N == BigInteger.Zero);
                
            byte[] encodedR = EncodeEd25519Point(R);
            byte[] encodedPubKey = EncodeEd25519Point(key.Y);
            byte[] rv = encodedR.Concat(encodedPubKey).Concat(message).ToArray();
            // h = hash(R + pubKey + msg) mod q
            var h = GetM(rv) % Curve.N;
            // s = (r + h * privKey) mod q
            s = (r + (key.X * h) % Curve.N) % Curve.N;
            
            return (new Point(R.GetX(), R.GetY()), s);
        }

        /// <summary>
        /// Encoding a Ed25519 point
        /// </summary>
        /// <param name="point"></param>
        /// <returns>A byte[] of encoded point.</returns>
        public static byte[] EncodeEd25519Point(Point point)
        {
            var x_lsb = point.GetX() & 1;
            // Encode the y-coordinate as a little-endian string of 32 octets.
            byte[] yByteArray = point.GetY().ToByteArray(true, false).PadRight(32);
            int new_msb = 0;
            if (x_lsb == 1)
            {
                int mask = 128;
                new_msb = yByteArray[31] | mask;
            }
            if (x_lsb == 0)
            {
                int mask = 127;
                new_msb = yByteArray[31] & mask;
            }
            // Copy the least significant bit of the x - coordinate to the most significant bit of the final octet.
            yByteArray[31] = (byte)new_msb;
            return yByteArray;
        }

        /// <summary>
        /// EdDSA verification with point Ed25519
        /// </summary>
        /// <param name="message"></param>
        /// <param name="R"></param>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <returns>A Boolean : True if the verification is successful , else => false.</returns>
        public static bool Ed25519Verify(byte[] message, Point R, BigInteger s, Key key)
        {
            byte[] encodedR = EncodeEd25519Point(R);
            byte[] encodedPubKey = EncodeEd25519Point(key.Y);
            byte[] rv = encodedR.Concat(encodedPubKey).Concat(message).ToArray();
            // h = hash(R + pubKey + msg) mod q
            var h = GetM(rv) % Curve.N;
            var p1 = Curve.G * s;
            var p2 = R + (key.Y * h);

            return p1.GetX() == p2.GetX();
        }

        /// <summary>
        /// Hashing with SHA512
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A BigInteger.</returns>
        public static BigInteger GetM(byte[] message)
        {
            return new BigInteger(Utils.HashSHA512(message).ToArray(), true, false);
        }
    }
