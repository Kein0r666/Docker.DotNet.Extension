using Docker.DotNet.Extension.Models.Enums;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class PortBuild
    {
        /// <summary>
        /// Internal (private) port into container => <port>/<tcp|udp|sctp>
        /// </summary>
        public string InternalPort { get; set; }
        /// <summary>
        /// External (public) ports into container => <port>
        /// </summary>
        public ExternalPortBinding[] ExternalPorts { get; set; }
        /// <summary>
        /// Port type 
        /// </summary>
        public PortType Type { get; set; }

        public PortBuild()
        {

        }
        public PortBuild(string internalPort, PortType portType, params ExternalPortBinding[] externalPorts)
        {
            InternalPort = internalPort;
            ExternalPorts = externalPorts;
            Type = portType;
        }

        public KeyValuePair<string, EmptyStruct> GetExposedPorts()
        {
            return new KeyValuePair<string, EmptyStruct>(GetInternalPort(), new EmptyStruct());
        }

        public KeyValuePair<string, IList<PortBinding>> GetPortBuild()
        {
            return new KeyValuePair<string, IList<PortBinding>>(GetInternalPort(), GetExternalPort());
        }

        public string GetInternalPort()
        {
            return $"{InternalPort}/{Type.ToString("G").ToLower()}";
        }

        public List<PortBinding> GetExternalPort()
        {
            return ExternalPorts
                .Select(x => new PortBinding { HostIP = x.HostIp, HostPort = x.HostPort })
                .ToList();
        }
    }
}
