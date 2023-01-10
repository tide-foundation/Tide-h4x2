using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using H4x2_TinySDK.Tools;
using H4x2_TinySDK.Ed25519;

namespace H4x2_Simulator.Entities;

public class Ork
{
	public string OrkId { get; set; }
	public string OrkPub { get; set; }
	public string OrkUrl { get; set; }
	public string SignedOrkUrl { get; set; }
	public BigInteger GetOrkId()
	{
		if(OrkId is null) throw new Exception("Ork Class: OrkId is null");
		return BigInteger.Parse(OrkId);
	}
	public void SetOrkId(BigInteger orkId) => OrkId = orkId.ToString();
	public Point GetOrkPub()
	{
		if(OrkPub is null) throw new Exception("Ork Class: OrkPub is null");
		return Point.FromBase64(OrkPub);
	}
	public void SetOrkPub(Point orkPub) => OrkPub = orkPub.ToBase64();
	public static BigInteger GenerateID(string data) =>  Utils.Mod(new BigInteger(SHA256.HashData(Encoding.ASCII.GetBytes(data)), false, true), Curve.N);
}

