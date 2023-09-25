using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Users.Dto
{
    public class ForgotPasswordDto
    {
        [Required]
        public string EmployeeCode { get; set; }

        [Required]
        public int PasswordStatus { get; set; }
    }
}
