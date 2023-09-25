namespace MobiVUE.Utility
{
    public static class CommonFunction
    {
        private static string Key = "BCIL";

        public static string Encrypt(string value)
        {
            return Crypto.EncryptStringAES(value, Key);
        }

        public static string Decrypt(string value)
        {
            return Crypto.DecryptStringAES(value, Key);
        }
    }
}