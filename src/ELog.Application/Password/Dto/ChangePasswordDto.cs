using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Password.Dto
{
    public class ChangePasswordDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        [Required]
        public int PasswordStatus { get; set; }
        [Required]
        public int CurrentUser { get; set; }
        public bool Status { get; set; }
        public string UserName { get; set; }
    }
}
