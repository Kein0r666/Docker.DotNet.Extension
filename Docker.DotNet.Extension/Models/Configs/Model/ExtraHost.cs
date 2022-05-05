using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class ExtraHost
    {
        public string Ip { get; set; }
        public string Domain { get; set; }

        public ExtraHost()
        {

        }
        public ExtraHost(string ip, string domain)
        {
            Ip = ip;
            Domain = domain;
        }

        public string GetExtraHost()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(":", Domain, Ip);
            return sb.ToString();
        }
    }
}
