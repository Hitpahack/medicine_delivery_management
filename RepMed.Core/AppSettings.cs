using System;
using System.Collections.Generic;
using System.Text;

namespace RepMed.Core
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string AppSecreateKey { get; set; }
        public string MainSiteURL { get; set; }
        public string AdminSiteURL { get; set; }
        public string AdminEmail { get; set; }
        public string[] AllowOriginsUrls { get; set; }
        public bool RequestLog { get; set; }
        public JwtAuth JwtAuth { get; set; }
        public IList<string> Languages { get; set; }
    }

    public class JwtAuth
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
