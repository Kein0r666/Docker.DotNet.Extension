using Docker.DotNet.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Configs.Model
{
    public class Variable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Variable()
        {

        }
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string GetVariable()
        {
            var sb = new StringBuilder();
            sb.AppendJoin("=", Name, Value);
            return sb.ToString();
        }
    }
}
