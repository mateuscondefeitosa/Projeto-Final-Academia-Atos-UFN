namespace DreamsMade.Crypto
{
    public interface ICrypto
    {
        string Encrypt(string password);

        string Decrypt(string password);
    }
}
