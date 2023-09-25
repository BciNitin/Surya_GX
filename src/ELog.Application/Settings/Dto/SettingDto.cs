using Abp.AutoMapper;
using Abp.Configuration;

namespace ELog.Application.Settings.Dto
{
    [AutoMapFrom(typeof(Setting))]
    public class SettingDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}