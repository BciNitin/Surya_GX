using Microsoft.PowerBI.Api.Models;
using System;

namespace ELog.Web.Host.Startup
{
    public class EmbedConfig
    {
        public string EmbedURL { get; internal set; }
        public Guid GroupID { get; internal set; }
        public Report ReportData { get; internal set; }
        public Guid ReportID { get; internal set; }
        public string Token { get; internal set; }
        public Guid? TokenID { get; internal set; }
        public DateTime? Expiration { get; internal set; }
    }
}