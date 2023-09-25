using System.ComponentModel.DataAnnotations;

namespace ELog.Application.Hardware.Dto
{
    public class Connection
    {
        [Required]
        public string IPAddress { get; set; }
        [Required]
        public int Port { get; set; }
    }
}
