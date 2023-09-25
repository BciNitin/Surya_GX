using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Password.Dto
{
    public class ChangePasswordSuperAdminDto
    {
        [Required]
        public string EmployeeCode { get; set; }
        [Required]
        public string NewPassword { get; set; }

    }
}
