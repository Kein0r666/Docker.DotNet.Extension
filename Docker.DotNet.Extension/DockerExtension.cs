using Docker.DotNet;
using Docker.DotNet.Extension;
using Docker.DotNet.Extension.Models.Configs;
using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Extension.Models.Enums;
using Docker.DotNet.Extension.Models.Response;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.DotNet.Extension
{
    public static class DockerExtension
    {
        /// <summary>
        /// Create new container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="configs">Config for start an container</param>
        /// <param name="customConfig">Custom config for HostConfig</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Container creation response</returns>
        public static async Task<CreateContainerResponse> CreateContainerAsync(this DockerClient client,
            ContainerConfigs configs,
            CustomConfig customConfig = null,
            CancellationToken cancellationToken = default)
            => await client.Containers.CreateContainerAsync(configs.CreateConfigs(customConfig), cancellationToken);

        /// <summary>
        /// Create new container by another container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="imageName">Name of image</param>
        /// <param name="container">Docker container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Container creation response</returns>
        public static async Task<CreateContainerResponse> CopyContainerAsync(this DockerClient client,
            string imageName,
            ContainerInspectResponse container,
            CancellationToken cancellationToken = default)
            => await client.Containers.CreateContainerAsync(container.GetConfigs(imageName), cancellationToken);

        /// <summary>
        /// Start container by container response
        /// </summary>
        /// <param name="client">Docker clients</param>
        /// <param name="container">Container response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container successfully started, otherwise <see langword="false"/></returns>
        public static async Task<bool> StartContainerAsync(this DockerClient client,
            ContainerListResponse container,
            CancellationToken cancellationToken = default)
            => await client.StartContainerAsync(container.ID, cancellationToken);

        /// <summary>
        /// Start container 
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Id of container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container successfully started, otherwise <see langword="false"/></returns>
        public static async Task<bool> StartContainerAsync(this DockerClient client,
            string containerId,
            CancellationToken cancellationToken = default)
            => await client.Containers.StartContainerAsync(containerId, new ContainerStartParameters(), cancellationToken);

        /// <summary>
        /// Stop container by container response
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container response</param>
        /// <param name="waitSeconds">Wait before kill (seconds)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container successfully stopped, otherwise <see langword="false"/></returns>
        public static async Task<bool> StopContainerAsync(this DockerClient client,
            ContainerListResponse container,
            uint waitSeconds,
            CancellationToken cancellationToken = default)
            => await client.StopContainerAsync(container.ID, waitSeconds, cancellationToken);

        /// <summary>
        /// Stop container 
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Id container</param>
        /// <param name="waitSeconds">Wait before kill (seconds)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container successfully stopped, otherwise <see langword="false"/></returns>
        public static async Task<bool> StopContainerAsync(this DockerClient client,
            string containerId,
            uint? waitSeconds = null,
            CancellationToken cancellationToken = default)
            => await client.Containers.StopContainerAsync(containerId, new ContainerStopParameters { WaitBeforeKillSeconds = waitSeconds }, cancellationToken);

        /// <summary>
        /// Remove container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container response</param>
        /// <param name="force">Force remove container</param>
        /// <param name="removeVolume">Remove volume</param>
        /// <param name="removeLinks">Remove lings</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task RemoveContainerAsync(this DockerClient client,
            ContainerListResponse container,
            bool? force = null,
            bool? removeVolume = null,
            bool? removeLinks = null,
            CancellationToken cancellationToken = default)
            => await client.RemoveContainerAsync(container.ID, force, removeVolume, removeLinks, cancellationToken);

        /// <summary>
        /// Remove container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Container id</param>
        /// <param name="force">Force remove container</param>
        /// <param name="removeVolume">Remove volume</param>
        /// <param name="removeLinks">Remove links</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task RemoveContainerAsync(this DockerClient client,
            string containerId,
            bool? force = null,
            bool? removeVolume = null,
            bool? removeLinks = null,
            CancellationToken cancellationToken = default)
            => await client.Containers.RemoveContainerAsync(containerId, new ContainerRemoveParameters
            {
                Force = force,
                RemoveLinks = removeLinks,
                RemoveVolumes = removeVolume,
            }, cancellationToken);

        /// <summary>
        /// Restart container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task RestartContainerAsync(this DockerClient client,
            ContainerListResponse container,
            CancellationToken cancellationToken = default)
            => await client.RestartContainerAsync(container.ID, cancellationToken);

        /// <summary>
        /// Restart container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Container id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task RestartContainerAsync(this DockerClient client,
            string containerId,
            CancellationToken cancellationToken = default)
            => await client.Containers.RestartContainerAsync(containerId, new ContainerRestartParameters(), cancellationToken);

        /// <summary>
        /// Create and start new container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="configs">Config for start an container</param>
        /// <param name="customConfig">Custom configs for HostConfig</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns><see langword="true"/> if container was successfully created and started, otherwise <see langword="false"/></returns>
        public static async Task<bool> ContainerUpAsync(this DockerClient client,
            ContainerConfigs configs,
            CustomConfig customConfig,
            CancellationToken cancellationToken = default)
        {
            var container = await client.CreateContainerAsync(configs, customConfig, cancellationToken);
            return await client.StartContainerAsync(container.ID, cancellationToken);
        }

        /// <summary>
        /// Create and start new container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerInspect">Container inspect information</param>
        /// <param name="imageName">Image name</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns><see langword="true"/> if container was successfully created and started, otherwise <see langword="false"/></returns>
        public static async Task<bool> ContainerUpAsync(this DockerClient client,
            string imageName,
            ContainerInspectResponse containerInspect,
            CancellationToken cancellationToken = default)
        {
            var container = await client.CopyContainerAsync(imageName, containerInspect, cancellationToken);
            return await client.StartContainerAsync(container.ID, cancellationToken);
        }

        /// <summary>
        /// Recreating container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerNameOrId">Name or id of container</param>
        /// <param name="imageName">Name of image</param>
        /// <param name="force">Force recreating container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> is container successful recreating, otherwise <see langword="false"/></returns>
        public static async Task<bool> RecreateContainerAsync(this DockerClient client,
            string containerNameOrId,
            string imageName,
            bool force = true,
            CancellationToken cancellationToken = default)
        {
            var container = await client.Containers.GetContainerAsync(containerNameOrId, cancellationToken);

            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var info = await client.Containers.InspectContainerAsync(container.ID, cancellationToken);

            await client.RemoveContainerAsync(container, force, cancellationToken: cancellationToken);
            return await client.ContainerUpAsync(imageName, info, cancellationToken);
        }

        /// <summary>
        /// Update container with new (or old) image
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container</param>
        /// <param name="imageName">New image name</param>
        /// <param name="force">Force recreating container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container was updated, otherwise <see langword="false"/></returns>
        public static async Task<bool> UpdateContainerAsync(this DockerClient client,
            ContainerListResponse container,
            string imageName,
            bool force = true,
            CancellationToken cancellationToken = default)
        {
            //Name of old image
            var imageDown = container.Image;
            //Name of new image
            var imageUp = imageName;

            //Getting name of container
            var containerName = container.Names.FirstOrDefault();
            try
            {
                //Recreating container with new image
                await client.RecreateContainerAsync(containerName, imageUp, force, cancellationToken);

                //Check if container was successfuly started, if true recreate container with old image
                if (!await client.Containers.CheckContainerStatusAsync(containerName, State.Running, cancellationToken))
                {
                    //Recreate container with old image
                    await client.RecreateContainerAsync(containerName, imageDown, force, cancellationToken);
                }
            }
            catch (Exception)
            {
                //If trowed any exception, try to recreate container with old image
                await client.RecreateContainerAsync(containerName, imageDown, force, cancellationToken);
                throw;
            }

            return true;
        }

        /// <summary>
        /// Build image
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="config">Config for creation docker image</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if image was created, otherwise <see langword="false"/></returns>
        public static async Task<bool> BuildImageAsync(this DockerClient client,
            BuildImageConfig config,
            CancellationToken cancellationToken = default)
        {
            if (!config.Dockerfile.Contains("Dockerfile"))
                config.Dockerfile = Path.Combine(config.Dockerfile, "Dockerfile");

            using (var fs = new FileStream(config.PathToTgz, FileMode.Open, FileAccess.Read))
            {
                await client.Images.BuildImageFromDockerfileAsync(config, fs, config.AuthConfig, config.Headers, config.Progress, cancellationToken);
            }

            if (config.Prune.HasValue && config.Prune.Value)
                await client.Images.PruneImagesAsync(cancellationToken: cancellationToken);


            return true;
        }

        /// <summary>
        /// Tag image
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="oldImageName">Old image tag</param>
        /// <param name="newImageName">New image tag</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task TagImageAsync(this DockerClient client,
            ImageTag oldImageName,
            ImageTag newImageName,
            CancellationToken cancellationToken = default)
        {
            await client.Images.TagImageAsync(oldImageName.GetImageTag(), new ImageTagParameters
            {
                RepositoryName = newImageName.Name,
                Tag = newImageName.Tag
            }, cancellationToken);
        }

        /// <summary>
        /// Tag image by repository name (for push image)
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="repositoryName">Repository name</param>
        /// <param name="imageTag">Image tag</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task TagPushingImageAsync(this DockerClient client,
            string repositoryName,
            ImageTag imageTag,
            CancellationToken cancellationToken = default)
            => await client.TagImageAsync(imageTag, new ImageTag($"{repositoryName}/{imageTag.Name}", imageTag.Tag), cancellationToken);

        /// <summary>
        /// Pulling image from repository
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="config">Pull config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Name of pulled image</returns>
        public static async Task<PullImageResponse> PullImageAsync(this DockerClient client,
            PullImageConfig config,
            CancellationToken cancellationToken = default)
        {
            var imageName = config.GetPullImageName();
            await client.Images.CreateImageAsync(new ImagesCreateParameters
            {
                FromImage = imageName,
            },
            config.AuthConfig,
            config.Progress,
            cancellationToken);

            return new PullImageResponse
            {
                PulledImageName = imageName,
            };
        }

        /// <summary>
        /// Pushing image to repository
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="config">Push config</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task PushImageAsync(this DockerClient client,
            PushImageConfig config,
            CancellationToken cancellationToken = default)
        {
            await client.Images.PushImageAsync(config.GetPushImageName(), new ImagePushParameters
            {
                Tag = config.ImageTag.Tag
            },
            config.AuthConfig,
            config.Progress,
            cancellationToken);
        }

        /// <summary>
        /// Execute an command into container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container response</param>
        /// <param name="workingDir">Path to workking directory</param>
        /// <param name="commands">Array of commands</param>
        /// <param name="privileged">Use privileged commands</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static async Task<string> ExecCommandContainerAsync(this DockerClient client,
            ContainerListResponse container,
            string[] commands,
            string workingDir = null,
            bool privileged = true,
            CancellationToken cancellationToken = default)
            => await client.ExecCommandContainerAsync(container.ID, commands, workingDir, privileged, cancellationToken);

        /// <summary>
        /// Execute an command into container
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Container id</param>
        /// <param name="workingDir">Path to workking directory</param>
        /// <param name="commands">Array of commands</param>
        /// <param name="privileged">Use privileged commands</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public static async Task<string> ExecCommandContainerAsync(this DockerClient client,
            string containerId,
            string[] commands,
            string workingDir = null,
            bool privileged = true,
            CancellationToken cancellationToken = default)
        {
            var response = await client.Exec.ExecCreateContainerAsync(containerId, new ContainerExecCreateParameters
            {
                Cmd = commands,
                Privileged = privileged,
                WorkingDir = workingDir,
            }, cancellationToken);

            return response.ID;
        }

        /// <summary>
        /// Get container stats
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="container">Container response</param>
        /// <param name="progress">Progress to send information about container</param>
        /// <param name="isStream">Activate stream</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static async Task GetContainerStatsAsync(this DockerClient client,
            ContainerListResponse container,
            IProgress<ContainerStatsResponse> progress,
            bool isStream = true,
            CancellationToken cancellationToken = default)
            => await client.GetContainerStatsAsync(container.ID, progress, isStream, cancellationToken);

        /// <summary>
        /// Get container stats
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="containerId">Container id</param>
        /// <param name="progress">Progress to send information about container</param>
        /// <param name="isStream">Activate stream</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public static async Task GetContainerStatsAsync(this DockerClient client,
            string containerId,
            IProgress<ContainerStatsResponse> progress,
            bool isStream = true,
            CancellationToken cancellationToken = default)
            => await client.Containers.GetContainerStatsAsync(containerId, new ContainerStatsParameters { Stream = isStream }, progress, cancellationToken);
    }
}
