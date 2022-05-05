using Docker.DotNet.Extension;
using Docker.DotNet.Extension.Models.Configs;
using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Docker.DotNet.Extension
{
    public static class DockerConfigurationExtension
    {
        /// <summary>
        /// Get configs by another container
        /// </summary>
        /// <param name="container">Container info</param>
        /// <param name="imageName">Name of image, if name image is null then configs created by old image</param>
        /// <returns>Config for create container</returns>
        public static CreateContainerParameters GetConfigs(this ContainerInspectResponse container, string imageName = null)
        {
            return new CreateContainerParameters(container.Config)
            {
                Image = imageName ?? container.Image,
                Name = container.Name,
                ExposedPorts = container.Config.ExposedPorts,
                HostConfig = container.HostConfig,
                Env = container.Config.Env
            };
        }

        /// <summary>
        /// Create configs for create a new container
        /// </summary>
        /// <param name="containerConfigs">Container configs</param>
        /// <returns>Creation config container</returns>
        public static CreateContainerParameters CreateConfigs(this ContainerConfigs containerConfigs, CustomConfig customConfig = null)
        {
            return new CreateContainerParameters
            {
                Image = containerConfigs.ImageName,
                Name = containerConfigs.ContainerName,
                ExposedPorts = containerConfigs.GetExposedPorts(),
                HostConfig = containerConfigs.GetHostConfig(customConfig),
                Env = containerConfigs.GetEnvironmentVariable()
            };
        }

        /// <summary>
        /// Get config for creation a container
        /// </summary>
        /// <param name="containerConfigs">Container configs</param>
        /// <param name="customConfig">Custom configs</param>
        /// <returns>Host config</returns>
        public static HostConfig GetHostConfig(this ContainerConfigs containerConfigs, CustomConfig customConfig = null)
        {
            return new HostConfig(containerConfigs.Resources)
               .SetExtraHosts(containerConfigs.ExtraHosts)
               .SetVolumes(containerConfigs.Volumes)
               .SetRestartPolicy(containerConfigs.RestartPolicy)
               .SetPorts(containerConfigs.Ports)
               .SetCustomConfig(customConfig);
        }

        /// <summary>
        /// Set extra hosts to host config
        /// </summary>
        /// <param name="config">Host config</param>
        /// <param name="extraHosts">List of extra hosts</param>
        /// <returns>List extra hosts</returns>
        public static HostConfig SetExtraHosts(this HostConfig config, params ExtraHost[] extraHosts)
        {
            if (extraHosts == null)
                return config;

            config.ExtraHosts = extraHosts.Select(x => x.GetExtraHost()).ToList();
            return config;
        }

        /// <summary>
        /// Set volumes into host config
        /// </summary>
        /// <param name="config">Host config</param>
        /// <param name="volumes">Array of volumes path (formate <path</param>
        /// <returns>Host config</returns>
        public static HostConfig SetVolumes(this HostConfig config, params Volume[] volumes)
        {
            if (volumes == null)
                return config;

            config.Binds = volumes.Select(x => x.GetVolume()).ToList();
            return config;
        }

        /// <summary>
        /// Set restart on state
        /// </summary>
        /// <param name="config">Host config</param>
        /// <param name="restartPolicy">Restart policy</param>
        /// <returns>Host config</returns>
        public static HostConfig SetRestartPolicy(this HostConfig config, ContainerRestartPolicy restartPolicy)
        {
            if (restartPolicy.RestartState == RestartPolicyKind.Undefined)
                return config;

            config.RestartPolicy = new RestartPolicy { Name = restartPolicy.RestartState, MaximumRetryCount = restartPolicy.CountRestart };
            return config;
        }

        /// <summary>
        /// Set ports to config
        /// </summary>
        /// <param name="config">Host config</param>
        /// <param name="ports">Array of ports</param>
        /// <returns>Host config</returns>
        public static HostConfig SetPorts(this HostConfig config, params PortBuild[] ports)
        {
            if (ports == null)
                return config;

            config.PortBindings = ports.Select(x => x.GetPortBuild()).ToDictionary(x => x.Key, x => x.Value);
            return config;
        }

        /// <summary>
        /// Set custom config for creation a container
        /// </summary>
        /// <param name="config">Config</param>
        /// <param name="customConfig">Custom configs</param>
        /// <returns>Configs</returns>
        public static HostConfig SetCustomConfig(this HostConfig config, CustomConfig customConfig)
        {
            if (customConfig == null)
                return config;

            var props = config.GetType().GetProperties();

            foreach (var customConfigs in customConfig.CustomConfigs)
            {
                foreach (var prop in props)
                {
                    if (prop.Name == customConfigs.Key)
                        prop.SetValue(customConfigs.Value, customConfigs);
                }
            }

            return config;
        }

        /// <summary>
        /// Get exposed(internal) ports
        /// </summary>
        /// <param name="containerConfigs">Configs/param>
        /// <returns>Exposed ports</returns>
        private static Dictionary<string, EmptyStruct> GetExposedPorts(this ContainerConfigs containerConfigs)
            => containerConfigs.Ports.Select(x => x.GetExposedPorts()).ToDictionary(x => x.Key, x => x.Value);

        /// <summary>
        /// Get envoriment variable from config
        /// </summary>
        /// <param name="containerConfigs">Configs</param>
        /// <returns>List of environment variables</returns>
        private static IList<string> GetEnvironmentVariable(this ContainerConfigs containerConfigs)
            => containerConfigs.EnvironmentVariable.Select(x => x.GetVariable()).ToList();
    }
}
