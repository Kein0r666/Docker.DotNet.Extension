using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.System
{
    public class DockerSystemDf
    {
        public long LayersSize { get; set; }
        public DockerImage[] Images { get; set; }
        public DockerContainer[] Containers { get; set; }
        public DockerVolume[] Volumes { get; set; }
        public object BuildCache { get; set; }
        public int BuilderSize { get; set; }
    }

    public class DockerImage
    {
        public int Containers { get; set; }
        public int Created { get; set; }
        public string Id { get; set; }
        public object Labels { get; set; }
        public string ParentId { get; set; }
        public string[] RepoDigests { get; set; }
        public string[] RepoTags { get; set; }
        public int SharedSize { get; set; }
        public int Size { get; set; }
        public int VirtualSize { get; set; }
    }

    public class DockerContainer
    {
        public string Id { get; set; }
        public string[] Names { get; set; }
        public string Image { get; set; }
        public string ImageID { get; set; }
        public string Command { get; set; }
        public int Created { get; set; }
        public DockerPort[] Ports { get; set; }
        public int SizeRw { get; set; }
        public int SizeRootFs { get; set; }
        public DockerLabels Labels { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public DockerHostconfig HostConfig { get; set; }
        public DockerNetworksettings NetworkSettings { get; set; }
        public DockerMount[] Mounts { get; set; }
    }

    public class DockerLabels
    {
        public string comdockercomposeconfighash { get; set; }
        public string comdockercomposecontainernumber { get; set; }
        public string comdockercomposeoneoff { get; set; }
        public string comdockercomposeproject { get; set; }
        public string comdockercomposeprojectconfig_files { get; set; }
        public string comdockercomposeprojectworking_dir { get; set; }
        public string comdockercomposeservice { get; set; }
        public string comdockercomposeversion { get; set; }
    }

    public class DockerHostconfig
    {
        public string NetworkMode { get; set; }
    }

    public class DockerNetworksettings
    {
        public DockerNetworks Networks { get; set; }
    }

    public class DockerNetworks
    {
        public DockerBridge bridge { get; set; }
        public DockerRegister_Default register_default { get; set; }
    }

    public class DockerBridge
    {
        public object IPAMConfig { get; set; }
        public object Links { get; set; }
        public object Aliases { get; set; }
        public string NetworkID { get; set; }
        public string EndpointID { get; set; }
        public string Gateway { get; set; }
        public string IPAddress { get; set; }
        public int IPPrefixLen { get; set; }
        public string IPv6Gateway { get; set; }
        public string GlobalIPv6Address { get; set; }
        public int GlobalIPv6PrefixLen { get; set; }
        public string MacAddress { get; set; }
        public object DriverOpts { get; set; }
    }

    public class DockerRegister_Default
    {
        public object IPAMConfig { get; set; }
        public object Links { get; set; }
        public object Aliases { get; set; }
        public string NetworkID { get; set; }
        public string EndpointID { get; set; }
        public string Gateway { get; set; }
        public string IPAddress { get; set; }
        public int IPPrefixLen { get; set; }
        public string IPv6Gateway { get; set; }
        public string GlobalIPv6Address { get; set; }
        public int GlobalIPv6PrefixLen { get; set; }
        public string MacAddress { get; set; }
        public object DriverOpts { get; set; }
    }

    public class DockerPort
    {
        public string IP { get; set; }
        public int PrivatePort { get; set; }
        public int PublicPort { get; set; }
        public string Type { get; set; }
    }

    public class DockerMount
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Driver { get; set; }
        public string Mode { get; set; }
        public bool RW { get; set; }
        public string Propagation { get; set; }
    }

    public class DockerVolume
    {
        public DateTime CreatedAt { get; set; }
        public string Driver { get; set; }
        public object Labels { get; set; }
        public string Mountpoint { get; set; }
        public string Name { get; set; }
        public object Options { get; set; }
        public string Scope { get; set; }
        public DockerUsagedata UsageData { get; set; }
    }

    public class DockerUsagedata
    {
        public int RefCount { get; set; }
        public int Size { get; set; }
    }

}
