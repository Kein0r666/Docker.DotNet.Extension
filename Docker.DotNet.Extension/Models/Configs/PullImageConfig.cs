using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs
{
    public class PullImageConfig
    {
        public ImageTag ImageTag { get; set; }
        public AuthConfig AuthConfig { get; set; }
        public string RepositoryName { get; set; }
        public IProgress<JSONMessage> Progress { get; set; }

        public PullImageConfig()
        {
            Progress = new Progress<JSONMessage>();
        }

        public PullImageConfig(string repositoryName,
            ImageTag imageTag,
            AuthConfig authConfig,
            IProgress<JSONMessage> progress)
        {
            RepositoryName = repositoryName;
            ImageTag = imageTag;
            AuthConfig = authConfig;
            Progress = progress;
        }

        public string GetPullImageName()
        {
            return $"{RepositoryName}/{ImageTag.GetImageTag()}";
        }
    }
}
