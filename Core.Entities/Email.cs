using System;

namespace Core.Entities
{
    public class Email
    {
        public string From { get; set; }
        public String To { get; set; }
        public String Cc { get; set; }
        public String Bc { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public String Error { get; set; }
        public String Atachment { get; set; }
        public String Attachment_Images { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}
