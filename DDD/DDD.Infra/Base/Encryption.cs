using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DDD.Infra.Base
{
    public class Encryption
    {
        private const string myPassword = "minhasenha";
        private const string mySaltBase64 = "bWluaGFzZW5oYQ==";

        public static string Encrypt(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = Encoding.Unicode.GetBytes(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                var result = HttpServerUtility.UrlTokenEncode(ms.ToArray());

                return result;
            }
        }

        public static string Encrypt(Int32 inputInt)
        {
            string inputText = inputInt.ToString();

            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = Encoding.Unicode.GetBytes(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return HttpServerUtility.UrlTokenEncode(ms.ToArray());
            }
        }

        public static string DecryptToString(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = HttpServerUtility.UrlTokenDecode(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return Encoding.Unicode.GetString(ms.ToArray());
            }

        }

        public static Int32 DecryptToInt32(string inputText)
        {
            string password = myPassword;
            byte[] salt = Encoding.Unicode.GetBytes(mySaltBase64);

            var inputBytes = HttpServerUtility.UrlTokenDecode(inputText);

            var pdb = new Rfc2898DeriveBytes(password, salt);

            using (var ms = new MemoryStream())
            {
                var alg = Rijndael.Create();

                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);

                using (var cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                }

                return int.Parse(Encoding.Unicode.GetString(ms.ToArray()));
            }

        }
    }
}
