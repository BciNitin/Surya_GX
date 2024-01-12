using System;
using System.Collections.Generic;
using System.Text;

namespace MobiVueEVO.BO
{
    public class Response
    {
        public object? Result { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }
    }
}
