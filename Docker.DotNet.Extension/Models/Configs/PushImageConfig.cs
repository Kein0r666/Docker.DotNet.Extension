using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs
{
    public class PushImageConfig
    {
        public ImageTag ImageTag { get; set; }
        public string RepositoryName { get; set; }
        public AuthConfig AuthConfig { get; set; }
        public IProgress<JSONMessage> Progress { get; set; }

        public PushImageConfig()
        {
            Progress = new Progress<JSONMessage>();
        }
        public PushImageConfig(ImageTag imageTag,
            string repositoryName,
            AuthConfig authConfig,
            IProgress<JSONMessage> progress)
        {
            ImageTag = imageTag;
            RepositoryName = repositoryName;
            AuthConfig = authConfig;
            Progress = progress;
        }


        public string GetPushImageName()
        {
            return $"{RepositoryName}/{ImageTag.Name}";
        }
    }
}
