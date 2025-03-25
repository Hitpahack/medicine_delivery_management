using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Dtos
{
    public class XmlTemplateResponse
    {
        public XmlTemplateResponse(string error = "")
        {
            IsSuccess = false;
            Message = error;
        }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class OtpTemplate
    {
        public string SecurityCode { get; set; }
        public int Minute { get; set; }
    }
}
