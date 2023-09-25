namespace ELog.Web.Core.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }
        public string RefreshToken { get; set; }

        public long UserId { get; set; }
        public bool isMultiplePlantExists { get; set; }
        public int? plantId { get; set; }
        public int PasswordStatus { get; set; }

    }
}