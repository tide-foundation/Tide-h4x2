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

using System.Numerics;
using H4x2_TinySDK.Tools;
using H4x2_TinySDK.Math;
using System.Text;

namespace H4x2_TinySDK.Ed25519
{
    public class Key 
    {
        public BigInteger X { get; private set; }
        public Point Y { get; private set; }

        public Key()
        {
            X = Utils.RandomBigInt(Curve.N);
            Y = Curve.G * X;             
        }

        public Key(BigInteger x, Point y)
        {
            X = x;
            Y = y;
        }
        public Key(byte[] data) { SetBytes(data); }
        public Key(Point y) => Y = y;

        public static Key Public(BigInteger x, BigInteger y) => new Key(new Point(x, y));

        public static Key Private(BigInteger x, bool noPublic = false) =>
            new Key(x, noPublic ? Curve.G : Curve.G * x);

        public Key GetPublic() => new Key(this.Y);
        public static Key ParsePublic(byte[] data) => new Key(data);
        public static Key ParsePublic(string data) => new Key(Convert.FromBase64String(data));
        public static Key Parse(string data) => Parse(Convert.FromBase64String(data));
        public static Key Parse(byte[] data)
        {
            var x  = new BigInteger(data.Take(32).ToArray(), true, true);
            var y = Curve.G * x;
            return new Key(x,y); 
        } 
        private void SetBytes(IReadOnlyList<byte> bytes)
        {
            X = new BigInteger(bytes.Take(32).ToArray(), true, true);
            Y = Point.FromBytes(bytes.Skip(32).ToArray());
        }

        /// <summary>
        /// EdDSA signing with point Ed25519
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A byte[] signature with the combination of R and s.</returns>
        public byte[] EdDSASign(byte[] message)
        {
            var (R, s) = EdDSA.Ed25519Sign(message, this);
            
            return R.GetX().ToByteArray(true, false).PadRight(32).Concat(R.GetY().ToByteArray(true, false).PadRight(32))
               .Concat(s.ToByteArray(true, false).PadRight(32)).ToArray();

        }

        /// <summary>
        /// EdDSA verification with point Ed25519
        /// </summary>
        /// <param name="message"></param>
        /// <param name="signature"></param>
        /// <returns>Boolean : True if the varification is successful.</returns>
        public bool EdDSAVerify(byte[] message, byte[] signature)
        {
            if (signature == null || signature.Length != 96)
                return false;

            byte[] rByteArray = signature.Take(64).ToArray();
            var r_x = new BigInteger(rByteArray.Take(32).ToArray(), true, false);
            var r_y = new BigInteger(rByteArray.Skip(32).ToArray(), true, false);
            var s = new BigInteger(signature.Skip(64).ToArray(), true, false);
            Point rPoint = new Point(r_x, r_y);

            return EdDSA.Ed25519Verify(message, rPoint, s, this);
        }

        public bool EdDSAVerifyStr(string message, string signature)
        {
           return EdDSAVerify(Encoding.UTF8.GetBytes(message), Encoding.UTF8.GetBytes(signature));
        }
     
     
    }
}
