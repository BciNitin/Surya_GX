using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}