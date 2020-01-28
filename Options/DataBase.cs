using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Options
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string Name { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }

    public interface IDataBaseSettings
    {
        string Name { get; set; }
        string ConnectionString { get; set; }
    }
}