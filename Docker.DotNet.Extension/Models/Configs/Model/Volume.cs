using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class Volume
    {
        public string HostPath { get; set; }
        public string ContainerPath { get; set; }

        public Volume()
        {

        }
        public Volume(string hostPath, string containerPath)
        {
            HostPath = hostPath;
            ContainerPath = containerPath;
        }

        public string GetVolume()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(":", HostPath, ContainerPath);

            return sb.ToString();
        }
    }
}
