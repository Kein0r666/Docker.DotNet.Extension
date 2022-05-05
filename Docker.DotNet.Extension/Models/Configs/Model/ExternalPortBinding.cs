using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class ExternalPortBinding
    {
        public string HostIp { get; set; }
        public string HostPort { get; set; }
        public ExternalPortBinding()
        {

        }
        public ExternalPortBinding(string ip, string port)
        {
            HostIp = ip;
            HostPort = port;
        }
    }
}
