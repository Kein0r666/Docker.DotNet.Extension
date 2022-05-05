using Docker.DotNet;
using Docker.DotNet.Extension;
using Docker.DotNet.Extension.Models.Enums;
using Docker.DotNet.Extension.Models.Params;
using Docker.DotNet.Extension.Models.System;
using Docker.DotNet.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Docker.DotNet.Extension
{
    public static class DockerInfoExtension
    {

        /// <summary>
        /// Getting containers
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="since">Container name or id</param>
        /// <param name="all">Get all containers</param>
        /// <param name="param">Custom paramters for searching containers</param>
        /// <returns>List of containers</returns>
        private static async Task<IList<ContainerListResponse>> GetContainerCoreAsync(this IContainerOperations container,
            string since = null,
            bool? all = null,
            Param[] param = null,
            CancellationToken cancellationToken = default)
            => await container.ListContainersAsync(new ContainersListParameters
            {
                All = all,
                Since = since,
                Filters = DockerCommonExtension.CreateFilter(param)
            }, cancellationToken);

        /// <summary>
        /// Getting images
        /// </summary>
        /// <param name="image">Docker image interface</param>
        /// <param name="all">Get all images</param>
        /// <param name="param">Custom param for searching images</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List images</returns>
        private static async Task<IList<ImagesListResponse>> GetImagesAsync(this IImageOperations image,
            bool? all = null,
            Param[] param = null,
            CancellationToken cancellationToken = default)
            => await image.ListImagesAsync(new ImagesListParameters
            {
                All = all,
                Filters = DockerCommonExtension.CreateFilter(param)
            }, cancellationToken);

        /// <summary>
        /// Getting containers by custom params
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="all">Get all containers</param>
        /// <param name="param">Custom parameters for searching containers</param>
        /// <returns>List of containers</returns>
        public static async Task<IList<ContainerListResponse>> GetListContainersByFilterAsync(this IContainerOperations container, bool all = true, CancellationToken cancellationToken = default, params Param[] param)
            => await container.GetContainerCoreAsync(all: all, param: param, cancellationToken: cancellationToken);

        /// <summary>
        /// Gettinl all containers in docker
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="all">Get all containers</param>
        /// <returns>List of containers</returns>
        public static async Task<IList<ContainerListResponse>> GetListContainersAsync(this IContainerOperations container, bool all = true, CancellationToken cancellationToken = default)
            => await container.GetContainerCoreAsync(all: all, cancellationToken: cancellationToken);

        /// <summary>
        /// Get container by name or id
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="since">Container's name or id</param>
        /// <returns>Container</returns>
        /// <exception cref="ArgumentNullException">If docker return empty response</exception>
        public static async Task<ContainerListResponse> GetContainerAsync(this IContainerOperations container, string since, CancellationToken cancellationToken = default)
        {
            var containers = await container.GetContainerCoreAsync(since: since, cancellationToken: cancellationToken);
            return containers.FirstOrDefault() ?? throw new ArgumentNullException(nameof(containers));
        }

        /// <summary>
        /// Get container by image name
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="imageName">Name of image</param>
        /// <param name="all">Get all containers</param>
        /// <returns>List child containers of requested image</returns>
        public static async Task<IList<ContainerListResponse>> GetListContainersByImageAsync(this IContainerOperations container, string imageName, bool all = true, CancellationToken cancellationToken = default)
            => await container.GetListContainersByFilterAsync(all, cancellationToken, new Param("ancestors", imageName));

        /// <summary>
        /// Getting list running containers
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="all">Get all containers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List containers</returns>
        public static async Task<IList<ContainerListResponse>> GetListRunningContainers(this IContainerOperations container, bool all = true, CancellationToken cancellationToken = default)
            => await container.GetListContainersByFilterAsync(all, cancellationToken, new Param("status", State.Running.ToName()));

        /// <summary>
        /// Inspect container 
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="containerResponse">Container response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Inspect result</returns>
        public static async Task<ContainerInspectResponse> InspectContainerAsync(this IContainerOperations container, ContainerListResponse containerResponse, CancellationToken cancellationToken = default)
            => await container.InspectContainerAsync(containerResponse.ID, cancellationToken);

        /// <summary>
        /// Check container status
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="containerResponse">Container response</param>
        /// <param name="status">Status of container</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container with this name and status exist, otherwise <see langword="false"/> </returns>
        public static async Task<bool> CheckContainerStatusAsync(this IContainerOperations container, ContainerListResponse containerResponse, State status, CancellationToken cancellationToken = default)
            => await container.CheckContainerStatusAsync(containerResponse.ID, status, cancellationToken);

        /// <summary>
        /// Check container status
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="since">Container id</param>
        /// <param name="status">Status of container</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see langword="true"/> if container with this name and status exist, otherwise <see langword="false"/></returns>
        public static async Task<bool> CheckContainerStatusAsync(this IContainerOperations container, string since, State status, CancellationToken cancellationToken = default)
        {
            var containers = await container.GetContainerCoreAsync(
                since: since,
                all: true,
                param: new Param[] { new Param("status", status.ToName()) },
                cancellationToken: cancellationToken);

            return containers.Any();
        }

        /// <summary>
        /// Check if container exist
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="containerResponse">Container response</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container exist, otherwise <see langword="false"/></returns>
        public static async Task<bool> ContainerExistAsync(this IContainerOperations container, ContainerListResponse containerResponse, CancellationToken cancellationToken = default)
            => await container.ContainerExistAsync(containerResponse.ID, cancellationToken);
        /// <summary>
        /// Check if container exist
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="since">Container id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns><see langword="true"/> if container exist, otherwise <see langword="false"/></returns>
        public static async Task<bool> ContainerExistAsync(this IContainerOperations container, string since, CancellationToken cancellationToken = default)
        {
            var containers = await container.GetContainerCoreAsync(
                since: since,
                all: true,
                cancellationToken: cancellationToken);

            return containers.Any();
        }

        /// <summary>
        /// Get file from container
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="containerResponse">Container output</param>
        /// <param name="pathIntoContainer">Path into container</param>
        /// <returns>Data of file</returns>
        public static async Task<string> GetFileFromContainerAsync(this IContainerOperations container, ContainerListResponse containerResponse, string pathIntoContainer)
        {
            var stream = await container.GetFileStreamFromContainerAsync(containerResponse, pathIntoContainer);
            using (var sr = new StreamReader(stream))
            {
                return await sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Get file stream from container
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="containerResponse">Container output</param>
        /// <param name="pathIntoContainer">Path into container</param>
        /// <returns>File stream</returns>
        public static async Task<Stream> GetFileStreamFromContainerAsync(this IContainerOperations container, ContainerListResponse containerResponse, string pathIntoContainer)
            => await container.GetFileStreamFromContainerAsync(containerResponse.ID, pathIntoContainer);

        ///<summary>
        /// Get file stream from container
        /// </summary>
        /// <param name="container">Docker container interface</param>
        /// <param name="container">Container output</param>
        /// <param name="pathIntoContainer">Path into container</param>
        /// <returns>File stream</returns>
        public static async Task<Stream> GetFileStreamFromContainerAsync(this IContainerOperations container, string containerId, string pathIntoContainer)
        {
            var file = await container.GetArchiveFromContainerAsync(containerId, new GetArchiveFromContainerParameters { Path = pathIntoContainer }, true);
            return file.Stream;
        }

        /// <summary>
        /// Check if container is created from this image
        /// </summary>
        /// <param name="containers">List output containers</param>
        /// <param name="imageName">Image name</param>
        /// <returns>true if container is created by this image, otherwise false</returns>
        public static bool IsFromImage(this ContainerListResponse container, string imageName)
            => container.Image == imageName;

        /// <summary>
        /// Check if container is running
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is running</returns>
        public static bool IsRun(this ContainerListResponse container)
            => State.Running.Compare(container.State);

        /// <summary>
        /// Check if container is restarting
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is running</returns>
        public static bool IsRestarting(this ContainerListResponse container)
            => State.Restarting.Compare(container.State);

        /// <summary>
        /// Check if container is created
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is running</returns>
        public static bool IsCreated(this ContainerListResponse container)
            => State.Created.Compare(container.State);

        /// <summary>
        /// Check if container is removing
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is removing</returns>
        public static bool IsRemoving(this ContainerListResponse container)
            => State.Removing.Compare(container.State);

        /// <summary>
        /// Check if container is paused
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is paused</returns>
        public static bool IsPaused(this ContainerListResponse container)
            => State.Paused.Compare(container.State);

        /// <summary>
        /// Check if container is exited
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is exited</returns>
        public static bool IsExited(this ContainerListResponse container)
            => State.Exited.Compare(container.State);

        /// <summary>
        /// Check if container is dead
        /// </summary>
        /// <param name="container">Container</param>
        /// <returns>true if is dead</returns>
        public static bool IsDead(this ContainerListResponse container)
            => State.Dead.Compare(container.State);

        /// <summary>
        /// Getting images by filter
        /// </summary>
        /// <param name="image">Docker image interface</param>
        /// <param name="all">Get all images</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="param">List of filter params</param>
        /// <returns>List images</returns>
        public static async Task<IList<ImagesListResponse>> GetListImagesByFilterAsync(this IImageOperations image, bool all = true, CancellationToken cancellationToken = default, params Param[] param)
            => await image.GetImagesAsync(all: all, param: param, cancellationToken: cancellationToken);

        /// <summary>
        /// Getting images
        /// </summary>
        /// <param name="image">Docker image interface</param>
        /// <param name="all">Get all images</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List images</returns>
        public static async Task<IList<ImagesListResponse>> GetListImagesAsync(this IImageOperations image, bool all = true, CancellationToken cancellationToken = default)
            => await image.GetImagesAsync(all: all, cancellationToken: cancellationToken);

        /// <summary>
        /// Getting image by name, id or digest
        /// </summary>
        /// <param name="image">Docker image interface</param>
        /// <param name="since">Image name, id or digest</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Image</returns>
        /// <exception cref="ArgumentNullException">If docker return empty response</exception>
        public static async Task<ImagesListResponse> GetImageAsync(this IImageOperations image, string since, CancellationToken cancellationToken = default)
        {
            var images = await image.GetListImagesByFilterAsync(param: new Param("since", since), cancellationToken: cancellationToken);
            return images.FirstOrDefault() ?? throw new ArgumentNullException(nameof(images));
        }

        /// <summary>
        /// Getting danling images
        /// </summary>
        /// <param name="image">Docker image interface</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List images</returns>
        public static async Task<IList<ImagesListResponse>> GetDanglingImagesAsync(this IImageOperations image, CancellationToken cancellationToken = default)
            => await image.GetListImagesByFilterAsync(param: new Param("dangling", "true"), cancellationToken: cancellationToken);

        /// <summary>
        /// Get data usage information
        /// </summary>
        /// <param name="client">Docker client</param>
        /// <param name="certificate">Certificate for docker</param>
        /// <returns>Information of date usage</returns>
        public static async Task<DockerSystemDf> GetDockerSystemDfAsync(this DockerClient client, X509Certificate2 certificate = null)
        {
            var options = new RestClientOptions(client.Configuration.EndpointBaseUri.ToString());
            if (certificate != null)
                options.ClientCertificates.Add(certificate);

            var restClient = new RestClient(options);
            var restRequest = new RestRequest("system/df", Method.Get);
            var response = await restClient.ExecuteAsync(restRequest);

            var df = JsonConvert.DeserializeObject<DockerSystemDf>(response.Content);
            return df;
        }

    }
}
