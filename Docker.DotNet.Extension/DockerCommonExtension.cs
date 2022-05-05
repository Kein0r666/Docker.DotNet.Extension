using Docker.DotNet.Extension;
using Docker.DotNet.Extension.Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Docker.DotNet.Extension
{
    public static class DockerCommonExtension
    {
        public static IDictionary<string, IDictionary<string, bool>> CreateFilter(string name, string value)
            => CreateFilter(new Param(name, value));

        public static IDictionary<string, IDictionary<string, bool>> CreateFilter(params (string name, string value)[] param)
            => CreateFilter(param.Select(x => new Param(x.name, x.value)).ToArray());

        public static IDictionary<string, IDictionary<string, bool>> CreateFilter(params Param[] param)
            => param.Select(x => x.GetFilter()).ToDictionary(x => x.Key, x => x.Value);

        public static IDictionary<string, IDictionary<string, bool>> CreateFilter(string name, string value, bool isActive)
            => CreateFilter(new Param(name, value, isActive));

        private static KeyValuePair<string, IDictionary<string, bool>> GetFilter(this Param param)
            => new KeyValuePair<string, IDictionary<string, bool>>(
                param.Name,
                new Dictionary<string, bool>
                {
                    { param.Value, param.IsActive }
                });

        internal static StringBuilder AppendJoin(this StringBuilder stringBuilder, string separator, params string[] values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (values.Length == 0)
                return stringBuilder;

            if (values[0] != null)
            {
                stringBuilder.Append(values[0].ToString());
            }

            for (var i = 1; i < values.Length; i++)
            {
                stringBuilder.Append(separator);
                if (values[i] != null)
                {
                    stringBuilder.Append(values[i].ToString());
                }
            }

            return stringBuilder;
        }
    }
}
