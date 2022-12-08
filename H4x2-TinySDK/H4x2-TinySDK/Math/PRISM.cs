using H4x2_TinySDK.Ed25519;
using System.Numerics;

namespace H4x2_TinySDK.Math
{
    public class PRISM
    {
        public static Point Apply(Point toApply, BigInteger scalar) => toApply * scalar;
    }
}
