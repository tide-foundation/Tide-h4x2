using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace H4x2_TinySDK.Tools
{
    public class AES
    {
        public static string Encrypt(string plaintext, string key) => Encrypt(plaintext, Convert.FromBase64String(key));
        public static string Encrypt(string plaintext, BigInteger key) => Encrypt(plaintext, key.ToByteArray(true, false));
        public static string Encrypt(string plaintext, byte[] key) // check this works with JS
        {
            using (var aes = new AesGcm(key))
            {
                var nonce = new byte[AesGcm.NonceByteSizes.MaxSize];
                RandomNumberGenerator.Fill(nonce);

                var tag = new byte[AesGcm.TagByteSizes.MaxSize];

                var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                var ciphertext = new byte[plaintextBytes.Length];

                aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);

                var encryptedData = Convert.ToBase64String(nonce.Concat(ciphertext).Concat(tag).ToArray());

                return encryptedData;
            }
        }
        public static string Decrypt(string ciphertext, BigInteger key) => Decrypt(ciphertext, key.ToByteArray(true, false));
        public static string Decrypt(string ciphertext, byte[] key)
        {
            using (var aes = new AesGcm(key))
            {
                var cipherbytes = Convert.FromBase64String(ciphertext);
                var plaintextBytes = new byte[ciphertext.Length - 28];  // Nonce is 12B + Tag whcih is 16B

                var nonce = cipherbytes.Take(12).ToArray();
                var tag = cipherbytes.Skip(nonce.Length + plaintextBytes.Length).ToArray();

                aes.Decrypt(nonce, cipherbytes, tag, plaintextBytes);

                return Encoding.UTF8.GetString(plaintextBytes);
            }
        }
    }
}
