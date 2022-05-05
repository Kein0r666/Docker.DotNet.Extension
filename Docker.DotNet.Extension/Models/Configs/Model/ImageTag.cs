using Docker.DotNet.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class ImageTag
    {
        public string Name { get; set; }
        public string Tag { get; set; }

        public ImageTag()
        {

        }
        public ImageTag(string name, string tag)
        {
            Name = name;
            Tag = tag;
        }

        public string GetImageTag()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(":", Name, Tag);
            return sb.ToString();
        }
    }
}
