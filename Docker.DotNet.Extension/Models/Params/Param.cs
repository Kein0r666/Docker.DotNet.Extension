using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.DotNet.Extension.Models.Params
{
    public class Param
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }

        public Param(string name, string value, bool isActive = true)
        {
            Name = name;
            Value = value;
            IsActive = isActive;
        }
    }
}
