using System;

namespace EnvironmentVariableExplorer.Models
{
    public class EnvironmentVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public EnvironmentVariableTarget Target { get; set; }
    }
}
