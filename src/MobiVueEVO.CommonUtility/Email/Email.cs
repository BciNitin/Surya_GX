using System.Collections.Generic;

namespace MobiVUE.Utility
{
    public class Email
    {
        public List<KeyValue<string, string>> Receivers { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}