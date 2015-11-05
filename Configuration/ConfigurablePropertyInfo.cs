using System;
using System.Reflection;

namespace ModularFramework.Configuration
{
    public class ConfigurablePropertyInfo
    {
        public ConfigurablePropertyInfo(PropertyInfo pi, ConfigurablePropertyAttribute cpa)
        {
            Name = cpa.Name;
            PropertyInfo = pi;
            ElementType = pi.DeclaringType;
        }

        public bool IsDefined(object elem)
        {
            return ElementType.IsAssignableFrom(elem.GetType()) && elem.GetType().GetProperty(PropertyInfo.Name) != null;
        }

        public string Name { get; }
        public PropertyInfo PropertyInfo { get; }
        public Type ElementType { get; }

    }
}
