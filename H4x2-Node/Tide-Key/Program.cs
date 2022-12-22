using H4x2_TinySDK.Ed25519;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0) { throw new ArgumentException("Specify whether you want to generate or sign"); }
        if (args[0].Equals("generate"))
        {
            Console.WriteLine(PrivateKey.Generate());
        }
        if (args[0].Equals("sign"))
        {
            if (args.Length < 3) { throw new ArgumentException("Must provide message to sign and private key"); }
            var priv = new PrivateKey(args[1]);
            Console.WriteLine(priv.Sign(args[2]));
        }
        if (args[0].Equals("pubhash"))
        {
            if (args.Length < 2) { throw new ArgumentException("Must provide message to sign and private key"); }
            var priv = new PrivateKey(args[1]);
            Console.WriteLine(priv.Public().ToUID());
        }
    }
}