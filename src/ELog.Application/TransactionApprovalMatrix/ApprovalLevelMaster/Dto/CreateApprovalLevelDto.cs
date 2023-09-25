using System.ComponentModel.DataAnnotations;

namespace ELog.Application.TransactionApprovalMatrix.ApprovalLevel.Dto
{
    public class CreateApprovalLevelDto
    {
        [Required(ErrorMessage = "Approval level code is required.")]
        public int LevelCode { get; set; }

        [Required(ErrorMessage = "Approval level name is required.")]
        public string LevelName { get; set; }


        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}