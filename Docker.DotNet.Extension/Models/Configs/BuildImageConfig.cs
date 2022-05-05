using Docker.DotNet.Extension.Models.Configs.Model;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs
{
    public class BuildImageConfig : ImageBuildParameters
    {
        public BuildImageConfig()
        {
            AuthConfig = new List<AuthConfig>();
            Headers = new Dictionary<string, string>();
            Progress = new Progress<JSONMessage>();
        }

        public BuildImageConfig(string pathToTgz, string pathIntoTgz = "./Dockerfile")
        {
            PathToTgz = pathToTgz;
            Dockerfile = pathIntoTgz;
            AuthConfig = new List<AuthConfig>();
            Headers = new Dictionary<string, string>();
            Progress = new Progress<JSONMessage>();
        }
        public BuildImageConfig(
            string pathToTgz,
            IEnumerable<ExtraHost> extraHosts,
            IEnumerable<ImageTag> imageTags,
            IEnumerable<AuthConfig> authConfigs = null,
            Dictionary<string, string> headers = null,
            IProgress<JSONMessage> progress = null,
            string pathIntoTgz = "./Dockerfile",
            bool prune = false,
            bool noCache = true)
        {
            PathToTgz = pathToTgz;
            Dockerfile = pathIntoTgz;
            NoCache = noCache;
            AuthConfig = authConfigs.ToList();
            Headers = headers;
            Progress = progress;
            Prune = prune;
            ExtraHosts = extraHosts.Select(x => x.GetExtraHost()).ToList();
            Tags = imageTags.Select(x => x.GetImageTag()).ToList();
        }

        public bool? Prune { get; set; }
        public string PathToTgz { get; set; }
        public List<AuthConfig> AuthConfig { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public IProgress<JSONMessage> Progress { get; set; }

        public BuildImageConfig AddAuthConfig(AuthConfig authConfig)
        {
            if (AuthConfig == null)
                throw new ArgumentNullException(nameof(AuthConfig));

            AuthConfig.Add(authConfig);
            return this;
        }
        public BuildImageConfig AddHeader(string name, string value)
        {
            if (Headers == null)
                throw new ArgumentNullException(nameof(Headers));

            Headers.Add(name, value);
            return this;
        }
        public BuildImageConfig AddProgerss(IProgress<JSONMessage> progress)
        {
            Progress = progress;
            return this;
        }
    }
}
