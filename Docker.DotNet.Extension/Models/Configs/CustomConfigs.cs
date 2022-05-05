using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs
{
    public class CustomConfig
    {
        /// <summary>
        /// Key is name of parameter value is value of parameter
        /// </summary>
        public Dictionary<string, object> CustomConfigs { get; set; }

        public CustomConfig()
        {

        }
        public CustomConfig(params Tuple<string, object>[] configs)
        {
            CustomConfigs = configs.ToDictionary(x => x.Item1, x => x.Item2);
        }
    }
}
