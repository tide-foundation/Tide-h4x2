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
using System.Text.Json.Serialization;
using System.Text.Json;
using H4x2_TinySDK.Ed25519;

namespace H4x2_Node.Models.Serialization
{
    public class PointSerializer : JsonConverter<Point>
    {
        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var val = reader.GetString();
            if (string.IsNullOrEmpty(val)) return new Point(BigInteger.Zero, BigInteger.One, BigInteger.One, BigInteger.Zero); //infinity also known as identity for ed25519

            Point point;
            try
            {
                point = Point.FromBytes(Convert.FromBase64String(val));
            }
            catch (ArgumentException)
            {
                throw new JsonException($"Unable to convert \"{val}\" into a point");
            }

            if (!point.IsSafePoint())
            {
                throw new JsonException($"Invalid point for \"{val}\"");
            }

            return point;
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
            => writer.WriteStringValue(Convert.ToBase64String(value.ToByteArray()));
    }
}
