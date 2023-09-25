namespace ELog.Web.Core.Models.TokenAuth
{
    public class AccessCodeValidationResponse
    {
        public AccessCodeValidationResponse()
        {
            IsValid = true;
        }

        public bool IsValid
        {
            get; set;
        }

        public string Email
        {
            get; set;
        }

        public string UserId
        {
            get; set;
        }
    }
}
