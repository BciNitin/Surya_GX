using System;
using System.Net;

namespace ELog.Application.CommonDto
{
    public class HTTPResponseDto
    {
        public int Result { get; set; } = (int)HttpStatusCode.OK;
        public string Error { get; set; }
        public Object ResultObject { get; set; }
        public string Status { get; set; }
    }
}