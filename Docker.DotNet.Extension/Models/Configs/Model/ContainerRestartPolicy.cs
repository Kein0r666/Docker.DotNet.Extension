using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class ContainerRestartPolicy
    {
        public RestartPolicyKind RestartState { get; set; }
        public int CountRestart { get; set; }

        public ContainerRestartPolicy()
        {

        }
        public ContainerRestartPolicy(RestartPolicyKind restartPolicy, int countRestart)
        {
            RestartState = restartPolicy;
            CountRestart = countRestart;
        }
    }
}
