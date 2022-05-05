using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs
{
    public class ContainerConfigs
    {
        public ContainerConfigs(string imageName, string containerName)
        {
            ImageName = imageName;
            ContainerName = containerName;
        }

        public string ImageName { get; set; }
        public string ContainerName { get; set; }
        public PortBuild[] Ports { get; set; }
        public Volume[] Volumes { get; set; }
        public Variable[] EnvironmentVariable { get; set; }
        public ExtraHost[] ExtraHosts { get; set; }
        public ContainerRestartPolicy RestartPolicy { get; set; }
        public Resources Resources { get; set; }
    }
}
