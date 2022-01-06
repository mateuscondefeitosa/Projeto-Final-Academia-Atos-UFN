using System.Security.Cryptography;
using System.Text;

namespace DreamsMade.Crypto
{
    public class Crypto: ICrypto
    {
        private const string PublicKey = "12345678";
        private const string SecretKey = "87654321";

        public string Encrypt(string Senha)
        {
            try
            {
                string ToReturn = "";
                byte[] secretkeyByte = { };
                secretkeyByte = Encoding.UTF8.GetBytes(SecretKey);
                byte[] publickeybyte = { };
                publickeybyte = Encoding.UTF8.GetBytes(PublicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = Encoding.UTF8.GetBytes(Senha);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public string Decrypt(string Senha)
        {
            try
            {
                string ToReturn = "";
                byte[] privatekeyByte = { };
                privatekeyByte = Encoding.UTF8.GetBytes(SecretKey);
                byte[] publickeybyte = { };
                publickeybyte = Encoding.UTF8.GetBytes(PublicKey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[Senha.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(Senha.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
    }
}
