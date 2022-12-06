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

            if (!point.IsValid())
            {
                throw new JsonException($"Invalid point for \"{val}\"");
            }

            return point;
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
            => writer.WriteStringValue(Convert.ToBase64String(value.ToByteArray()));
    }
}
