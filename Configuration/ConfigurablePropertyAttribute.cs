using System;

namespace ModularFramework.Configuration
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ConfigurablePropertyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
